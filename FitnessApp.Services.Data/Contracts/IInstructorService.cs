using FitnessApp.Data.Models;
using FitnessApp.Web.ViewModels.Instructor;

namespace FitnessApp.Services.Data.Contracts
{
    public interface IInstructorService
    {
        Task<bool> ExistsByIdAsync(int userId);
        Task<bool> ExistsByUserIdAsync(string userId);

        Task CreateAsync(BecomeInstructorFormModel model, string userId);

        Task<int?> GetInstructorByIdAsync(string userId);

        Task<bool> UserWithLicenseNumberExistsInDbAsync(int licenseNumber);

        bool UserWithLicenseNumberExistsGlobally(int licenseNumber);

        Task<bool> IsLicenseNumberValidAsync(int licenseNumber);

        Task<double> GetRatingByIdAsync(string userId);

        public Task<Instructor> GetByIdAsync(int userId);

        public Task<InstructorViewModel> GetInstructorViewModelByIdAsync(int userId);

        public Task Rate(InstructorRateFormModel model, int instructorId);

        public Task EditBiographyAsync(InstructorEditBiographyFormModel model, int instructorId);
        public Task EditSpecializationsAsync(InstructorEditSpecializationsFormModel model, int instructorId);
    }
}
