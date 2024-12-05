using FitnessApp.Data.Models;
using FitnessApp.Data.Repository.Contracts;
using FitnessApp.Services.Data.Contracts;
using FitnessApp.Web.ViewModels.Category;
using FitnessApp.Web.ViewModels.FitnessClass;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using FitnessApp.Web.Infrastructure.Enumerations;
using FitnessApp.Web.ViewModels.Instructor;
using FitnessApp.Web.ViewModels.Review;
using FitnessApp.Web.ViewModels.Booking;
using System.Text.RegularExpressions;

namespace FitnessApp.Services.Data
{
    public class FitnessClassService : IFitnessClassService
    {
        private readonly IRepository repository;

        public FitnessClassService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<Guid> AddClassAsync(FitnessClassFormModel model, int instructorId)
        {

            bool tryParse = DateTime.TryParseExact(model.StartTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

            var capacity = model.Capacity;
            FitnessClass fitnessClass = new FitnessClass()
            {
                Title = model.Title,
                InstructorId = instructorId,
                CategoryId = model.CategoryId,
                Description = model.Description,
                StartTime = date,
                Duration = model.Duration,
                Capacity = model.Capacity,
                LeftCapacity = model.Capacity,
                IsApproved = false
            };

            var statuses = AllStatuses();
            Status status = statuses.FirstOrDefault(s => s.Name == "Active");

            fitnessClass.StatusId = status.Id;

            await repository.AddAsync(fitnessClass);
            await repository.SaveChangesAsync();
            return fitnessClass.Id;
        }

        public async Task<FitnessClassQueryServiceModel> AllAsync(string? category = null,
            string? status = null,
            string? searchTerm = null,
            FitnessClassSorting sorting = FitnessClassSorting.StartTime,
            int currentPage = 1,
            int classesPerPage = 1)
        {
            var classesToShow = repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.IsApproved == true); //add approved by admin

            if (category != null)
            {
                classesToShow = classesToShow.Where(c => c.Category.Name == category);
            }

            if (status != null)
            {
                if (status == "Active")
                {
                    classesToShow = classesToShow.Where(c => c.StatusId == 1);
                }
                else if (status == "Canceled")
                {
                    classesToShow = classesToShow.Where(c => c.StatusId == 2);
                }
                else if (status == "Finished")
                {
                    classesToShow = classesToShow.Where(c => c.StatusId == 3);
                }
                else if (status == "Full")
                {
                    classesToShow = classesToShow.Where(c => c.StatusId == 4);
                }
            }

            if (searchTerm != null)
            {
                string normalizedSearchTerm = searchTerm.ToLower();
                classesToShow = classesToShow
                    .Where(c => (c.Title.ToLower().Contains(normalizedSearchTerm)) ||
                                c.Duration.ToString().Contains(normalizedSearchTerm) ||
                                c.Capacity.ToString().Contains(normalizedSearchTerm));
            }

            classesToShow = sorting switch
            {
                FitnessClassSorting.Duration => classesToShow.OrderBy(c => c.Duration),
                FitnessClassSorting.StartTime => classesToShow.OrderBy(c => c.StartTime),
                FitnessClassSorting.Capacity => classesToShow.OrderBy(c => c.Capacity),
                _ => classesToShow.OrderByDescending(c => c.Id),
            };

            IEnumerable<Status> statuses = AllStatuses();

            var classes = await classesToShow
                .Skip((currentPage - 1) * classesPerPage)
                .Take(classesPerPage)
                .Select(c => new FitnessClassServiceModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Duration = c.Duration,
                    Capacity = c.LeftCapacity,
                    Status = statuses.Where(s => s.Id == c.StatusId).Select(s => s.Name).First(),
                    InstructorFullName = c.Instructor.User.FirstName + " " + c.Instructor.User.LastName,
                    StartTime = c.StartTime.ToString("dd/MM/yyyy HH:mm"),
                    InstructorId = c.InstructorId,
                }).ToListAsync();

            int totalClasses = await classesToShow.CountAsync();

            return new FitnessClassQueryServiceModel()
            {
                FitnessClasses = classes,
                TotalClassesCount = totalClasses
            };
        }

        public async Task<IEnumerable<FitnessClassServiceModel>> AllBookedByUserId(string userId)
        {
            var user = await repository.GetByIdAsync<ApplicationUser>(userId);

            if (user == null)
            {
                throw new UnauthorizedAccessException("No such user exists!");
            }

            var bookings = await repository.AllReadOnly<Booking>()
                .ToListAsync();

            List<FitnessClassServiceModel> bookedClasses = new List<FitnessClassServiceModel>();

            foreach (var booking in bookings)
            {
                if (booking.UserId == userId)
                {
                    var fc = await repository.GetByIdAsync<FitnessClass>(booking.FitnessClassId);
                    var instructor = await repository.GetByIdAsync<Instructor>(fc.InstructorId);
                    var instructorUser = await repository.GetByIdAsync<ApplicationUser>(instructor.UserId);

                    if (fc.StartTime < DateTime.Now)
                    {
                        fc.StatusId = 3;
                    }

                    bookedClasses.Add(new FitnessClassServiceModel
                    {
                        Id = fc.Id,
                        Title = fc.Title,
                        Duration = fc.Duration,
                        Capacity = fc.LeftCapacity,
                        Status = AllStatuses().Where(s => s.Id == fc.StatusId).Select(s => s.Name).First(),
                        InstructorFullName = instructorUser.FirstName + " " + instructorUser.LastName,
                        StartTime = fc.StartTime.ToString(),
                    });
                }
            }
            return bookedClasses;
        }

        public async Task<IEnumerable<FitnessClassCategoryServiceModel>> AllCategoriesAsync()
        {
            return await repository.AllReadOnly<Category>()
                .Select(c => new FitnessClassCategoryServiceModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> AllCategoriesNamesAsync()
        {
            return await repository.AllReadOnly<Category>()
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<FitnessClassInstructorViewModel>> AllFitnessClassesByInstructorIdAsync(int instructorId)
        {
            return await repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.InstructorId == instructorId && fc.IsApproved == true)
                .Select(fc => new FitnessClassInstructorViewModel
                {
                    FitnessClassId = fc.Id,
                    Title = fc.Title,
                    LeftCapacity = fc.LeftCapacity,
                    Status = AllStatuses().Where(s => s.Id == fc.StatusId).Select(s => s.Name).First(),
                    StartTime = fc.StartTime.ToString("dd/MM/yyyy HH:mm")
                }).ToListAsync();
        }

        public async Task BookAsync(Guid id, string userId)
        {

            var fitnessClass = await repository.GetByIdAsync<FitnessClass>(id);
            var user = await repository.GetByIdAsync<ApplicationUser>(userId);

            if (fitnessClass != null && user != null)
            {
                if (fitnessClass.LeftCapacity - 1 >= 0)
                {
                    fitnessClass.LeftCapacity--;

                    if (fitnessClass.LeftCapacity == 0)
                    {
                        fitnessClass.StatusId = 4;
                    }

                    Booking booking = new()
                    {
                        UserId = userId,
                        FitnessClassId = fitnessClass.Id,
                        BookingDate = fitnessClass.StartTime
                    };

                    await repository.AddAsync<Booking>(booking);

                    await repository.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await repository.AllReadOnly<Category>()
                .AnyAsync(c => c.Id == categoryId);
        }

        public async Task DeleteAsync(Guid fitnessClassId)
        {
            var allClassReviews = await repository.All<Review>()
                .Where(r => r.FitnessClassId == fitnessClassId)
                .ToListAsync();

            if (allClassReviews.Any())
            {
                foreach (var review in allClassReviews)
                {
                    await repository.DeleteAsync<Review>(review.Id);
                }
            }

            var allClassBookings = await repository.All<Booking>()
                .Where(b => b.FitnessClassId == fitnessClassId)
                .ToListAsync();

            if (allClassBookings.Any())
            {
                foreach (var booking in allClassBookings)
                {
                    await repository.DeleteAsync<Booking>(booking.Id);
                }
            }

            await repository.DeleteAsync<FitnessClass>(fitnessClassId);
            await repository.SaveChangesAsync();
        }

        public async Task EditAsync(FitnessClassFormModel model)
        {
            var fitnessClassId = Guid.Parse(model.Id);
            var fitnessClass = await repository.GetByIdAsync<FitnessClass>(fitnessClassId);

            bool dateTime = DateTime.TryParseExact(model.StartTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);


            if (fitnessClass != null)
            {
                fitnessClass.Title = model.Title;
                fitnessClass.Capacity = model.Capacity;
                fitnessClass.LeftCapacity = model.Capacity;
                fitnessClass.Description = model.Description;
                fitnessClass.Duration = model.Duration;
                fitnessClass.CategoryId = model.CategoryId;
                fitnessClass.StartTime = date;
                fitnessClass.IsApproved = false;

                await repository.SaveChangesAsync();
            }


        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            bool reuslt = await repository.AllReadOnly<FitnessClass>()
                .AnyAsync(fc => fc.Id == id);

            return reuslt;
        }

        public async Task<FitnessClassDetailsServiceModel> FitnessClassDetailsByIdAsync(Guid id)
        {
            var fitnessClass = repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.Id == id);

            var reviews = repository.AllReadOnly<Review>()
                .Where(r => r.FitnessClassId == id)
                .Include(f => f.FitnessClass)
                .Include(r => r.FitnessClass.Instructor.User)
                .Select(r => new ReviewViewModel()
                {
                    InstructorFullName = r.FitnessClass.Instructor.User.FirstName + " " + r.FitnessClass.Instructor.User.LastName,
                    FitnessClassTitle = r.FitnessClass.Title,
                    Rating = r.Rating,
                    Comments = r.Comments,
                    ReviewerName = r.User.FirstName + " " + r.User.LastName,
                })
                .ToList();


            return await repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.Id == id)
                .Select(fc => new FitnessClassDetailsServiceModel
                {
                    Id = fc.Id,
                    Duration = fc.Duration,
                    Capacity = fc.Capacity,
                    Description = fc.Description,
                    Category = fc.Category.Name,
                    Title = fc.Title,
                    StartTime = fc.StartTime.ToString("dd/MM/yyyy HH:mm"),
                    Reviews = reviews,
                    Instructor = new InstructorServiceModel
                    {
                        Rating = fc.Instructor.Rating,
                        FullName = fc.Instructor.User.FirstName + " " + fc.Instructor.User.LastName
                    },
                    InstructorFullName = fc.Instructor.User.FirstName + " " + fc.Instructor.User.LastName
                })
                .FirstAsync();
        }

        public async Task<FitnessClass> GetByIdAsync(Guid fitnessClassId)
        {
            return await repository.GetByIdAsync<FitnessClass>(fitnessClassId);
        }

        public async Task<FitnessClassFormModel?> GetFitnessClassFormModelByIdAsync(string id)
        {
            return await repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.Id.ToString() == id)
                .Select(fc => new FitnessClassFormModel()
                {
                    Id = fc.Id.ToString(),
                    Title = fc.Title,
                    Description = fc.Description,
                    CategoryId = fc.CategoryId,
                    StartTime = Regex.Replace(fc.StartTime.ToString("dd/MM/yyyy HH:mm"), @"\.", "/"),
                    Duration = fc.Duration,
                    Capacity = fc.Capacity,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasInstructorWithIdAsync(Guid fitnessClassId, string userId)
        {
            return await repository.AllReadOnly<FitnessClass>()
                .AnyAsync(fc => fc.Instructor.UserId == userId
                                && fc.Id == fitnessClassId);
        }

        public async Task<bool> IsBookedByIUserWithIdAsync(Guid id, string userId)
        {
            return await repository.AllReadOnly<Booking>()
                .AnyAsync(b => b.UserId == userId && b.FitnessClassId == id);
        }

        public async Task<IEnumerable<FitnessClassIndexServiceModel>> LastFiveHousesAsync()
        {
            var classes = await repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.StatusId == 1 && fc.Capacity > 0 && fc.IsApproved == true)
                .Take(5)
                .Select(fc => new FitnessClassIndexServiceModel()
                {
                    Id = fc.Id.ToString(),
                    Title = fc.Title,
                    StartTime = fc.StartTime.ToString("dd/MM/yyyy HH:mm"),
                    Duration = fc.Duration.ToString(),
                    InstructorName = fc.Instructor.User.FirstName
                })
                .ToListAsync();

            return classes;
        }

        public async Task UnBookAsync(Guid id, string userId)
        {
            var fitnessClass = await repository.GetByIdAsync<FitnessClass>(id);

            var booking = await repository.AllReadOnly<Booking>()
                .FirstOrDefaultAsync(b => b.UserId == userId && b.FitnessClassId == id);

            if (fitnessClass != null)
            {
                if (booking == null)
                {
                    throw new UnauthorizedAccessException("This user has not booked this class");
                }

                if (fitnessClass.LeftCapacity + 1 > 0)
                {
                    fitnessClass.StatusId = 1;
                }

                fitnessClass.LeftCapacity += 1;

                await repository.DeleteAsync<Booking>(booking.Id);
                await repository.SaveChangesAsync();
            }
        }

        public async Task CancelClassAsync(Guid fitnessClassId)
        {
            var fitnessClass = await repository.GetByIdAsync<FitnessClass>(fitnessClassId);

            var statuses = AllStatuses();
            var canceledStatus = statuses.FirstOrDefault(s => s.Name == "Canceled");

            fitnessClass.StatusId = canceledStatus.Id;

            var bookings = await repository.All<Booking>()
                .Where(b => b.FitnessClassId == fitnessClassId)
                .ToListAsync();


            foreach (var booking in bookings)
            {
                await repository.DeleteAsync<Booking>(booking.Id);
            }

            await repository.SaveChangesAsync();
        }

        public IEnumerable<Status> AllStatuses()
        {
            return repository.AllReadOnly<Status>();
        }

        public async Task<IEnumerable<string>> AllStatusesNamesAsync()
        {
            return await repository.AllReadOnly<Status>()
                .Select(b => b.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task WriteReviewAsync(FitnessClassReviewFormModel model, string userId)
        {
            var review = new Review()
            {
                Rating = model.Rating,
                Comments = model.Comments,
                FitnessClassId = Guid.Parse(model.FitnessClassId),
                UserId = userId,
                DateSubmitted = DateTime.UtcNow,
                IsApproved = false,
            };

            await repository.AddAsync<Review>(review);
            await repository.SaveChangesAsync();
        }

        public async Task<bool> UserHasReviewedClassAsync(string userId, Guid fitnessClassId)
        {
            return await repository.AllReadOnly<Review>()
                .AnyAsync(r => r.FitnessClassId == fitnessClassId && r.UserId == userId);
        }

        public async Task<IEnumerable<FitnessClassServiceModel>> GetUnApprovedAsync()
        {
            var statuses = await repository.AllReadOnly<Status>().ToListAsync();

            return await repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.IsApproved == false)
                .Include(fc => fc.Status)
                .Include(fc => fc.Instructor)
                .Include(fc => fc.Instructor.User)
                .Select(fc => new FitnessClassServiceModel()
                {
                    Id = fc.Id,
                    Title = fc.Title,
                    Status = fc.Status.Name,
                    Duration = fc.Duration,
                    Capacity = fc.Capacity,
                    StartTime = fc.StartTime.ToString("dd/MM/yyyy HH:mm"),
                    InstructorId = fc.InstructorId,
                    InstructorFullName = fc.Instructor.User.FirstName + " " + fc.Instructor.User.LastName
                })
                .ToListAsync();

        }

        public async Task ApproveFitnessClassAsync(Guid fitnessClassId)
        {
            var fitnessClass = await repository.GetByIdAsync<FitnessClass>(fitnessClassId);

            if (fitnessClass != null && fitnessClass.IsApproved == false)
            {
                fitnessClass.IsApproved = true;

                await repository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ReviewViewModel>> AllReviewsAsync()
        {
            return await repository.AllReadOnly<Review>()
                .Include(r => r.User)
                .Include(r => r.FitnessClass)
                .Include(r => r.FitnessClass.Instructor.User)
                .Select(r => new ReviewViewModel()
                {
                    InstructorFullName = r.FitnessClass.Instructor.User.FirstName + " " + r.FitnessClass.Instructor.User.LastName,
                    FitnessClassTitle = r.FitnessClass.Title,
                    Rating = r.Rating,
                    ReviewerName = r.User.FirstName + " " + r.User.LastName,
                    Comments = r.Comments,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingViewModel>> AllBookingsAsync()
        {
            return await repository.AllReadOnly<Booking>()
                .Include(b => b.FitnessClass)
                .Include(b => b.FitnessClass.Instructor.User)
                .Include(b => b.User)
                .Select(b => new BookingViewModel()
                {
                    FitnessClassTitle = b.FitnessClass.Title,
                    BookerName = b.User.FirstName + " " + b.User.LastName,
                    InstructorFullName = b.FitnessClass.Instructor.User.FirstName + " " + b.FitnessClass.Instructor.User.LastName,
                    BookingDate = b.FitnessClass.StartTime.ToString("dd/MM/yyyy HH:mm")
                })
                .ToListAsync();
        }
    }
}
