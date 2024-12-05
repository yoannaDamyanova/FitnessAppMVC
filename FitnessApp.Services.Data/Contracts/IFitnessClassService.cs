using FitnessApp.Data.Models;
using FitnessApp.Web.Infrastructure.Enumerations;
using FitnessApp.Web.ViewModels.Category;
using FitnessApp.Web.ViewModels.FitnessClass;
using FitnessApp.Web.ViewModels.Review;
using FitnessApp.Web.ViewModels.Booking;

namespace FitnessApp.Services.Data.Contracts
{
    public interface IFitnessClassService
    {
        Task<IEnumerable<FitnessClassIndexServiceModel>> LastFiveHousesAsync();

        Task<IEnumerable<FitnessClassCategoryServiceModel>> AllCategoriesAsync();

        IEnumerable<Status> AllStatuses();

        Task<bool> CategoryExistsAsync(int categoryId);

        Task<Guid> AddClassAsync(FitnessClassFormModel model, int instructorId);

        Task<FitnessClassQueryServiceModel> AllAsync(
            string? category = null,
            string? status = null,
            string? searchTerm = null,
            FitnessClassSorting sorting = FitnessClassSorting.StartTime,
            int currentPage = 1,
            int housesPerPage = 1);

        Task<IEnumerable<string>> AllCategoriesNamesAsync();
        Task<IEnumerable<string>> AllStatusesNamesAsync();

        Task<IEnumerable<FitnessClassInstructorViewModel>> AllFitnessClassesByInstructorIdAsync(int instructorId);

        Task<IEnumerable<FitnessClassServiceModel>> AllBookedByUserId(string userId);

        Task<bool> ExistsAsync(Guid id);

        Task<FitnessClassDetailsServiceModel> FitnessClassDetailsByIdAsync(Guid id);

        Task EditAsync(FitnessClassFormModel model);

        Task<bool> HasInstructorWithIdAsync(Guid fitnessClassId, string userId);

        Task<FitnessClassFormModel?> GetFitnessClassFormModelByIdAsync(string id);

        Task DeleteAsync(Guid fitnessClassId);

        Task<bool> IsBookedByIUserWithIdAsync(Guid fitnessClassId, string userId);

        Task BookAsync(Guid id, string userId);

        Task UnBookAsync(Guid fitnessClassId, string userId);

        Task<IEnumerable<FitnessClassServiceModel>> GetUnApprovedAsync();

        Task ApproveFitnessClassAsync(Guid fitnessClassId);

        Task<FitnessClass> GetByIdAsync(Guid fitnessClassId);

        Task CancelClassAsync(Guid fitnessClassId);

        Task WriteReviewAsync(FitnessClassReviewFormModel model, string userId);

        Task<bool> UserHasReviewedClassAsync(string userId, Guid fitnessClassId);

        Task<IEnumerable<ReviewViewModel>> AllReviewsAsync();

        Task<IEnumerable<BookingViewModel>> AllBookingsAsync();
    }
}
