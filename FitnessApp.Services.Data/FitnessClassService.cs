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
            FitnessClassSorting sorting = FitnessClassSorting.Newest,
            int currentPage = 1,
            int classesPerPage = 1)
        {
            var classesToShow = repository.AllReadOnly<FitnessClass>(); //add approved by admin

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
                _ => classesToShow.OrderByDescending(c => c.Id),
            };

            foreach (var fc in classesToShow)
            {
                if (fc.StartTime < DateTime.Now)
                {
                    fc.StatusId = 3;
                }
                else if (fc.LeftCapacity <= 0)
                {
                    fc.StatusId = 4;
                }
            }

            IEnumerable<Status> statuses = AllStatuses();

            var classes = await classesToShow
                .Skip((currentPage - 1) * classesPerPage)
                .Take(classesPerPage)
                .Select(c => new FitnessClassServiceModel
                {
                    Id = c.Id.ToString(),
                    Title = c.Title,
                    Duration = c.Duration,
                    Capacity = c.LeftCapacity,
                    Status = statuses.Where(s => s.Id == c.StatusId).Select(s => s.Name).First(),
                    InstructorFullName = c.Instructor.User.FirstName + " " + c.Instructor.User.LastName,
                    StartTime = c.StartTime.ToString("dd/MM/yyyy HH:mm")
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
                    else if (fc.LeftCapacity <= 0)
                    {
                        fc.StatusId = 4;
                    }

                    bookedClasses.Add(new FitnessClassServiceModel
                    {
                        Id = fc.Id.ToString(),
                        Title = fc.Title,
                        Duration = fc.Duration,
                        Capacity = fc.Capacity,
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
                .Where(fc => fc.InstructorId == instructorId)
                .Select(fc => new FitnessClassInstructorViewModel
                {
                    FitnessClassId = fc.Id.ToString(),
                    Title = fc.Title,
                    LeftCapacity = fc.Capacity - fc.LeftCapacity,
                    Status = AllStatuses().Where(s => s.Id == fc.StatusId).Select(s => s.Name).First(),
                    StartTime = fc.StartTime.ToString("dd/MM/yyyy HH:mm")
                }).ToListAsync();
        }

        public async Task BookAsync(string id, string userId)
        {
            Guid fitnessClassId = Guid.Parse(id);
            var fitnessClass = await repository.GetByIdAsync<FitnessClass>(fitnessClassId);
            var user = await repository.GetByIdAsync<ApplicationUser>(userId);

            if (fitnessClass != null && user != null)
            {
                if (fitnessClass.LeftCapacity - 1 > 0)
                {
                    fitnessClass.LeftCapacity--;

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

        public async Task DeleteAsync(string fitnessClassId)
        {
            await repository.DeleteAsync<FitnessClass>(Guid.Parse(fitnessClassId));
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
                fitnessClass.Description = model.Description;
                fitnessClass.Duration = model.Duration;
                fitnessClass.CategoryId = model.CategoryId;
                fitnessClass.StartTime = date;
            }

            await repository.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string id)
        {
            Guid fitnessClassId = Guid.Parse(id);

            bool reuslt = await repository.AllReadOnly<FitnessClass>()
                .AnyAsync(fc => fc.Id == fitnessClassId);

            return reuslt;
        }

        public async Task<FitnessClassDetailsServiceModel> FitnessClassDetailsByIdAsync(string id)
        {
            var fitnessClass = repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.Id.ToString() == id);

            var reviews = repository.AllReadOnly<Review>()
                .Where(r => r.FitnessClassId.ToString() == id)
                .Select(r => new ReviewViewModel()
                {
                    Rating = r.Rating,
                    Comments = r.Comments,
                    ReviewerName = r.User.FirstName + " " + r.User.LastName,
                })
                .ToList();


            return await repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.Id.ToString() == id)
                .Select(fc => new FitnessClassDetailsServiceModel
                {
                    Id = fc.Id.ToString(),
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

        public async Task<FitnessClass> GetByIdAsync(string fitnessClassId)
        {
            return await repository.GetByIdAsync<FitnessClass>(Guid.Parse(fitnessClassId));
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
                    StartTime = fc.StartTime.ToString("dd/MM/yyyy HH:mm"),
                    Duration = fc.Duration,
                    Capacity = fc.Capacity,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasInstructorWithIdAsync(string fitnessClassId, string userId)
        {
            return await repository.AllReadOnly<FitnessClass>()
                .AnyAsync(fc => fc.Instructor.UserId == userId);
        }

        public async Task<bool> IsBookedByIUserWithIdAsync(string id, string userId)
        {
            Guid fitnessClassId = Guid.Parse(id);
            return await repository.AllReadOnly<Booking>()
                .AnyAsync(b => b.UserId == userId && b.FitnessClassId == fitnessClassId);
        }

        public async Task<IEnumerable<FitnessClassIndexServiceModel>> LastFiveHousesAsync()
        {
            var classes = await repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.StatusId == 1 && fc.Capacity > 0)
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

        public async Task UnBookAsync(string id, string userId)
        {
            Guid fitnessClassId = Guid.Parse(id);
            var fitnessClass = await repository.GetByIdAsync<FitnessClass>(fitnessClassId);

            var booking = await repository.AllReadOnly<Booking>()
                .FirstOrDefaultAsync(b => b.UserId == userId && b.FitnessClassId == fitnessClassId);

            if (fitnessClass != null)
            {
                if (booking == null)
                {
                    throw new UnauthorizedAccessException("This user has not booked this class");
                }

                fitnessClass.LeftCapacity += 1;

                await repository.DeleteAsync<Booking>(booking.Id);
                await repository.SaveChangesAsync();
            }
        }

        public async Task CancelClassAsync(string fitnessClassId)
        {
            var id = Guid.Parse(fitnessClassId);
            var fitnessClass = await repository.GetByIdAsync<FitnessClass>(id);

            var statuses = AllStatuses();
            var canceledStatus = statuses.FirstOrDefault(s => s.Name == "Canceled");

            fitnessClass.StatusId = canceledStatus.Id;

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
            };

            await repository.AddAsync<Review>(review);
            await repository.SaveChangesAsync();
        }

        public async Task<bool> UserHasReviewedClassAsync(string userId, string fitnessClassId)
        {
            return await repository.AllReadOnly<Review>()
                .AnyAsync(r=>r.FitnessClassId == Guid.Parse(fitnessClassId) && r.UserId == userId); 
        }
    }
}
