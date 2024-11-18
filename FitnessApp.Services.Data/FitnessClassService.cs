﻿using FitnessApp.Data.Models;
using FitnessApp.Data.Repository.Contracts;
using FitnessApp.Services.Data.Contracts;
using FitnessApp.Web.ViewModels.Category;
using FitnessApp.Web.ViewModels.FitnessClass;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using FitnessApp.Web.Infrastructure.Enumerations;

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
                FitnessClassSorting.Duration => classesToShow.OrderBy(c => c.Duration),
                FitnessClassSorting.StartTime => classesToShow.OrderBy(c => c.StartTime),
                FitnessClassSorting.Active => classesToShow.OrderBy(c => c.Status == true), //?
                _ => classesToShow.OrderByDescending(c => c.Id),
            };

            var classes = await classesToShow
                .Skip((currentPage - 1) * classesPerPage)
                .Take(classesPerPage)
                .Select(c => new FitnessClassServiceModel
                {
                    Id = c.Id.ToString(),
                    Duration = c.Duration,
                    Capacity = c.Capacity,
                    IsActive = c.Status,
                    InstructorFirstName = c.Instructor.User.FirstName,
                    InstructorLastName = c.Instructor.User.LastName,
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
                    InstructorFirstName = bfc.Instructor.User.FirstName,
                    InstructorLastName = bfc.Instructor.User.LastName,
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
                .Where(c => c.InstructorId == instructorId)
                .Select(c => new FitnessClassServiceModel
                {
                    Id = c.Id.ToString(),
                    Duration = c.Duration,
                    Capacity = c.Capacity,
                    IsActive = c.Status,
                    InstructorFirstName = c.Instructor.User.FirstName,
                    InstructorLastName = c.Instructor.User.LastName,
                    StartTime = c.StartTime.ToString("dd/MM/yyyy HH:mm")
                }).ToListAsync();
        }

        public async Task BookAsync(string id, string userId)
        {
            var fitnessClass = await repository.GetByIdAsync<FitnessClass>(id);

            if (fitnessClass != null)
            {
                fitnessClass.Bookings.Add(new Booking
                {
                    UserId = userId,
                    FitnessClassId = fitnessClass.Id,
                });
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
            await repository.DeleteAsync<FitnessClass>(fitnessClassId);
            await repository.SaveChangesAsync();
        }

        public async Task EditAsync(string fitnessClassId, FitnessClassFormModel model)
        {
            var fitnessClass = await repository.GetByIdAsync<FitnessClass>(fitnessClassId);

            bool dateTime = DateTime.TryParseExact(model.StartTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

            if (fitnessClass != null)
            {
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
            return await repository.AllReadOnly<FitnessClass>()
                .AnyAsync(fc => fc.Id.ToString() == id);
        }

        public async Task<FitnessClassDetailsServiceModel> FitnessClassDetailsByIdAsync(string id)
        {
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
                    InstructorFirstName = fc.Instructor.User.FirstName,
                    InstructorLastName = fc.Instructor.User.LastName,
                })
                .FirstAsync();
        }

        public async Task<FitnessClassFormModel?> GetFitnessClassFormModelByIdAsync(string id)
        {
            return await repository.AllReadOnly<FitnessClass>()
                .Where(fc => fc.Id.ToString() == id)
                .Select(fc => new FitnessClassFormModel()
                {
                    Title = fc.Title,
                    Description = fc.Description,
                    CategoryId = fc.CategoryId,
                    StartTime = fc.StartTime.ToString("dd/MM/yyyy HH:mm"),
                    Duration = fc.Duration,
                    Capacity = fc.Capacity,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasInstructorWithIdAsync(int fitnessClassId, string userId)
        {
            return await repository.AllReadOnly<FitnessClass>()
                .AnyAsync(fc => fc.Instructor.UserId == userId);
        }

        public async Task<IEnumerable<FitnessClassIndexServiceModel>> LastFiveHousesAsync()
        {
            return await repository.AllReadOnly<FitnessClass>()
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
        }

        public async Task UnBookAsync(string fitnessClassId, string userId)
        {
            var fitnessClass = await repository.GetByIdAsync<FitnessClass>(fitnessClassId);

            var booking = await repository.GetByIdAsync<Booking>(userId);

            if (fitnessClass != null)
            {
                if (booking == null)
                {
                    throw new UnauthorizedAccessException("This user has not booked this class");
                }

                fitnessClass.Bookings.Remove(booking);
                await repository.SaveChangesAsync();
            }
        }
    }
}
