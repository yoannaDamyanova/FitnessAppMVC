using FitnessApp.Web.ViewModels.Instructor;

namespace FitnessApp.Services.Data.Contracts
{
    public interface IInstructorService
    {
        Task<bool> ExistsByIdAsync(string userId);

        Task CreateAsync(BecomeInstructorFormModel model, string userId);

        Task<int?> GetInstructorByIdAsync(string userId);

        Task<bool> UserWithLicenseNumberExistsInDbAsync(int licenseNumber);

        bool UserWithLicenseNumberExistsGlobally(int licenseNumber);

        Task<bool> IsLicenseNumberValidAsync(int licenseNumber);

        Task<double> GetRatingByIdAsync(string userId);
    }
}
