﻿using FitnessApp.Data.Models;
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

            if (instructorId == 0)
            {

            }
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
                IsApproved = false,
                StatusId = 1
            };

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
                .Where(fc => fc.IsApproved == true)
                .Include(fc => fc.Instructor.User)
                .Include(fc => fc.Category)
                .Include(fc => fc.Status)
                .ToList();

            if (category != null)
            {
                classesToShow = classesToShow.Where(c => c.Category.Name == category).ToList();
            }

            if (status != null)
            {
                if (status == "Active")
                {
                    classesToShow = classesToShow.Where(c => c.StatusId == 1).ToList();
                }
                else if (status == "Canceled")
                {
                    classesToShow = classesToShow.Where(c => c.StatusId == 2).ToList();
                }
                else if (status == "Finished")
                {
                    classesToShow = classesToShow.Where(c => c.StatusId == 3).ToList();
                }
                else if (status == "Full")
                {
                    classesToShow = classesToShow.Where(c => c.StatusId == 4).ToList();
                }
            }

            if (searchTerm != null)
            {
                string normalizedSearchTerm = searchTerm.ToLower();
                classesToShow = classesToShow
                    .Where(c => (c.Title.ToLower().Contains(normalizedSearchTerm)) ||
                                c.Duration.ToString().Contains(normalizedSearchTerm) ||
                                c.Capacity.ToString().Contains(normalizedSearchTerm)).ToList();
            }

            classesToShow = sorting switch
            {
                FitnessClassSorting.Duration => classesToShow.OrderBy(c => c.Duration).ToList(),
                FitnessClassSorting.StartTime => classesToShow.OrderBy(c => c.StartTime).ToList(),
                FitnessClassSorting.Capacity => classesToShow.OrderBy(c => c.Capacity).ToList(),
                _ => classesToShow.OrderByDescending(c => c.Id).ToList(),
            };

            var classes = classesToShow
                .Skip((currentPage - 1) * classesPerPage)
                .Take(classesPerPage)
                .Select(c => new FitnessClassServiceModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Duration = c.Duration,
                    Capacity = c.LeftCapacity,
                    Status = c.Status.Name,
                    InstructorFullName = c.Instructor.User.FirstName + " " + c.Instructor.User.LastName,
                    StartTime = c.StartTime.ToString("dd/MM/yyyy HH:mm"),
                    InstructorId = c.InstructorId,
                }).ToList();

            int totalClasses = classesToShow.Count;

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

            var bookings = repository.AllReadOnly<Booking>()
                .Include(b => b.User)
                .Include(b => b.FitnessClass.Status)
                .Include(b => b.FitnessClass.Instructor.User)
                .ToList();

            List<FitnessClassServiceModel> bookedClasses = new List<FitnessClassServiceModel>();

            foreach (var booking in bookings)
            {
                if (booking.UserId == userId)
                {
                    var fc = booking.FitnessClass;
                    var instructorUser = booking.FitnessClass.Instructor.User;

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
                        Status = fc.Status.Name,
                        InstructorFullName = instructorUser.FirstName + " " + instructorUser.LastName,
                        StartTime = fc.StartTime.ToString("dd/MM/yyyy HH:mm"),
                    });
                }
            }
            return bookedClasses;
        }

        public async Task<IEnumerable<FitnessClassCategoryServiceModel>> AllCategoriesAsync()
        {
            return repository.AllReadOnly<Category>()
                .Select(c => new FitnessClassCategoryServiceModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToList();
        }

        public async Task<IEnumerable<string>> AllCategoriesNamesAsync()
        {
            return repository.AllReadOnly<Category>()
                .Select(c => c.Name)
                .Distinct()
                .ToList();
        }

        public async Task<IEnumerable<FitnessClassInstructorViewModel>> AllFitnessClassesByInstructorIdAsync(int instructorId)
        {
            return repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.InstructorId == instructorId && fc.IsApproved == true)
                .Include(fc => fc.Status)
                .Select(fc => new FitnessClassInstructorViewModel
                {
                    FitnessClassId = fc.Id,
                    Title = fc.Title,
                    LeftCapacity = fc.LeftCapacity,
                    Status = fc.Status.Name,
                    StartTime = fc.StartTime.ToString("dd/MM/yyyy HH:mm")
                }).ToList();
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
            return repository.AllReadOnly<Category>()
                .Any(c => c.Id == categoryId);
        }

        public async Task DeleteAsync(Guid fitnessClassId)
        {
            var allClassReviews = repository.All<Review>()
                .Where(r => r.FitnessClassId == fitnessClassId)
                .ToList();

            if (allClassReviews.Any())
            {
                foreach (var review in allClassReviews)
                {
                    await repository.DeleteAsync<Review>(review.Id);
                }
            }

            var allClassBookings = repository.All<Booking>()
                .Where(b => b.FitnessClassId == fitnessClassId)
                .ToList();

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
            bool reuslt = repository.AllReadOnly<FitnessClass>()
                .Any(fc => fc.Id == id);

            return reuslt;
        }

        public async Task<FitnessClassDetailsServiceModel> FitnessClassDetailsByIdAsync(Guid id)
        {
            var fitnessClass = repository.AllReadOnly<FitnessClass>()
                .Include(fc => fc.Instructor)
                .Include(fc => fc.Instructor.User)
                .FirstOrDefault(fc => fc.Id == id);

            if (fitnessClass == null)
            {
                throw new InvalidOperationException("This class doesn't exist!");
            }
            var reviews = repository.AllReadOnly<Review>()
                .Where(r => r.FitnessClassId == id)
                .Include(r => r.User)
                .Select(r => new ReviewViewModel()
                {
                    InstructorFullName = fitnessClass.Instructor.User.FirstName + " " + r.FitnessClass.Instructor.User.LastName,
                    FitnessClassTitle = fitnessClass.Title,
                    Rating = r.Rating,
                    Comments = r.Comments,
                    ReviewerName = r.User.FirstName + " " + r.User.LastName,
                })
                .ToList();

            return repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.Id == id)
                .Include(fc => fc.Status)
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
                    InstructorFullName = fc.Instructor.User.FirstName + " " + fc.Instructor.User.LastName,
                    Status = fc.Status.Name
                })
                .First();
        }

        public async Task<FitnessClass> GetByIdAsync(Guid fitnessClassId)
        {
            return await repository.GetByIdAsync<FitnessClass>(fitnessClassId);
        }

        public async Task<FitnessClassFormModel?> GetFitnessClassFormModelByIdAsync(string id)
        {
            return repository.AllReadOnly<FitnessClass>()
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
                .FirstOrDefault();
        }

        public async Task<bool> HasInstructorWithIdAsync(Guid fitnessClassId, string userId)
        {
            return repository.AllReadOnly<FitnessClass>()
                .Any(fc => fc.Instructor.UserId == userId
                                && fc.Id == fitnessClassId);
        }

        public async Task<bool> IsBookedByIUserWithIdAsync(Guid id, string userId)
        {
            return repository.AllReadOnly<Booking>()
                .Any(b => b.UserId == userId && b.FitnessClassId == id);
        }

        public async Task<IEnumerable<FitnessClassIndexServiceModel>> LastFiveHousesAsync()
        {
            var classes = repository.AllReadOnly<FitnessClass>()
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
                .ToList();

            return classes;
        }

        public async Task UnBookAsync(Guid id, string userId)
        {
            var fitnessClass = await repository.GetByIdAsync<FitnessClass>(id);

            var booking = repository.AllReadOnly<Booking>()
                .FirstOrDefault(b => b.UserId == userId && b.FitnessClassId == id);

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

            fitnessClass.StatusId = 2; // Canceled status id

            var bookings = repository.All<Booking>()
                .Where(b => b.FitnessClassId == fitnessClassId)
                .ToList();

            if (bookings.Any())
            {
                foreach (var booking in bookings)
                {
                    await repository.DeleteAsync<Booking>(booking.Id);
                }
            }

            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> AllStatusesNamesAsync()
        {
            return repository.AllReadOnly<Status>()
                .Select(b => b.Name)
                .Distinct()
                .ToList();
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
            return repository.AllReadOnly<Review>()
                .Any(r => r.FitnessClassId == fitnessClassId && r.UserId == userId);
        }

        public async Task<IEnumerable<FitnessClassServiceModel>> GetUnApprovedAsync()
        {
            var classes = repository.AllReadOnly<FitnessClass>()
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
                .ToList();

            return classes;
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
            return repository.AllReadOnly<Review>()
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
                .ToList();
        }

        public async Task<IEnumerable<BookingViewModel>> AllBookingsAsync()
        {
            return repository.AllReadOnly<Booking>()
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
                .ToList();
        }
    }
}
