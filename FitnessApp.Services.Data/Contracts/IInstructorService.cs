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

        Task<bool> UserWithLicenseNumberExistsInDbAsync(int licenseNumber);

        bool UserWithLicenseNumberExistsGlobally(int licenseNumber);

        Task<bool> IsLicenseNumberValidAsync(int licenseNumber);

        Task<double> GetRatingByIdAsync(string userId);
    }
}
