using FitnessApp.Data.Models;
using FitnessApp.Data.Repository.Contracts;
using FitnessApp.Services.Data.Contracts;
using FitnessApp.Web.ViewModels.Category;
using FitnessApp.Web.ViewModels.FitnessClass;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using FitnessApp.Web.Infrastructure.Enumerations;
using Azure.Core;
using FitnessApp.Web.ViewModels.Instructor;

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
                Status = true
            };

            await repository.AddAsync(fitnessClass);
            await repository.SaveChangesAsync();
            return fitnessClass.Id;
        }

        public async Task<FitnessClassQueryServiceModel> AllAsync(string? category = null,
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
                FitnessClassSorting.Active => (IQueryable<FitnessClass>)classesToShow.Select(c => c.Status == true),
                FitnessClassSorting.Canceled => (IQueryable<FitnessClass>)classesToShow.Select(c => c.Status == false),
                FitnessClassSorting.Duration => classesToShow.OrderBy(c => c.Duration),
                FitnessClassSorting.StartTime => classesToShow.OrderBy(c => c.StartTime),
                _ => classesToShow.OrderByDescending(c => c.Id),
            };

            var classes = await classesToShow
                .Skip((currentPage - 1) * classesPerPage)
                .Take(classesPerPage)
                .Select(c => new FitnessClassServiceModel
                {
                    Id = c.Id.ToString(),
                    Title = c.Title,
                    Duration = c.Duration,
                    Capacity = c.Capacity,
                    IsActive = c.Status,
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
            var bookedFitnessClasses = user.Bookings.Select(b => b.FitnessClass).ToList();

            return bookedFitnessClasses
                .Select(bfc => new FitnessClassServiceModel
                {
                    Id = bfc.Id.ToString(),
                    Duration = bfc.Duration,
                    Capacity = bfc.Capacity,
                    IsActive = bfc.Status,
                    InstructorFullName = bfc.Instructor.User.FirstName + " " + bfc.Instructor.User.LastName,
                    StartTime = bfc.StartTime.ToString("dd/MM/yyyy HH:mm")
                })
                .ToList();
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

        public async Task<IEnumerable<FitnessClassServiceModel>> AllFitnessClassesByInstructorIdAsync(int instructorId)
        {
            return await repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.InstructorId == instructorId)
                .Select(fc => new FitnessClassServiceModel
                {
                    Id = fc.Id.ToString(),
                    Title = fc.Title,
                    Duration = fc.Duration,
                    Capacity = fc.Capacity,
                    IsActive = fc.Status,
                    InstructorFullName = fc.Instructor.User.FirstName + " " + fc.Instructor.User.LastName,
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
                fitnessClass.Capacity--;

                Booking booking = new()
                {
                    UserId = userId,
                    FitnessClassId = fitnessClass.Id,
                    FitnessClass = fitnessClass,
                    User = user
                };

                fitnessClass.Bookings.Add(booking);

                await repository.SaveChangesAsync();
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
            return await repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.Id.ToString() == id)
                .Select(fc => new FitnessClassDetailsServiceModel
                {
                    Id = fc.Id.ToString(),
                    Duration = fc.Duration,
                    Capacity = fc.Capacity,
                    IsActive = fc.Status,
                    Description = fc.Description,
                    Category = fc.Category.Name,
                    Title = fc.Title,
                    StartTime = fc.StartTime.ToString("dd/MM/yyyy HH:mm"),
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
                .Where(fc => fc.Status == true && fc.Capacity > 0)
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

                fitnessClass.Capacity++;

                await repository.DeleteAsync<Booking>(booking.Id);
                await repository.SaveChangesAsync();
            }
        }
    }
}
