using FitnessApp.Data.Models;
using FitnessApp.Data.Repository.Contracts;
using FitnessApp.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Services.Data
{
    public class InstructorService : BaseService, IInstructorService
    {
        private readonly IRepository repository;

        public InstructorService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task CreateAsync(string userId)
        {
            await repository.AddAsync(new Instructor { UserId = userId, });
        }

        public async Task<bool> ExistsByIdAsync(string userId)
        {
            return await repository.AllReadOnly<Instructor>()
                .AnyAsync(a => a.UserId == userId); 
        }

        public async Task<int?> GetInstructorByIdAsync(string userId)
        {
            return (await repository.AllReadOnly<Instructor>()
                .FirstOrDefaultAsync(a => a.UserId == userId))?.Id;
        }

        public async Task<bool> UserWithLicenseNumberExistsAsync(int licenseNumber)
        {
            return await repository.AllReadOnly<Instructor>()
                .AnyAsync(i => i.LicenseNumber == licenseNumber);
        }
    }
}
