using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Services.Data.Contracts
{
    public interface IInstructorService
    {
        Task<bool> ExistsByIdAsync(string userId);

        Task CreateAsync(string userId);

        Task<int?> GetInstructorByIdAsync(string userId);

        Task<bool> UserWithLicenseNumberExistsAsync(int licenseNumber);
    }
}
