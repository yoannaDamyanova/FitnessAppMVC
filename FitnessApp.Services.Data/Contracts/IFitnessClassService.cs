using FitnessApp.Data.Models;
using FitnessApp.Web.Infrastructure.Enumerations;
using FitnessApp.Web.ViewModels.Category;
using FitnessApp.Web.ViewModels.FitnessClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            FitnessClassSorting sorting = FitnessClassSorting.Newest,
            int currentPage = 1,
            int housesPerPage = 1);

        Task<IEnumerable<string>> AllCategoriesNamesAsync();
        Task<IEnumerable<string>> AllStatusesNamesAsync();

        Task<IEnumerable<FitnessClassInstructorViewModel>> AllFitnessClassesByInstructorIdAsync(int instructorId);

        Task<IEnumerable<FitnessClassServiceModel>> AllBookedByUserId(string userId);

        Task<bool> ExistsAsync(string id);

        Task<FitnessClassDetailsServiceModel> FitnessClassDetailsByIdAsync(string id);

        Task EditAsync(FitnessClassFormModel model);

        Task<bool> HasInstructorWithIdAsync(string fitnessClassId, string userId);

        Task<FitnessClassFormModel?> GetFitnessClassFormModelByIdAsync(string id);

        Task DeleteAsync(string fitnessClassId);

        Task<bool> IsBookedByIUserWithIdAsync(string fitnessClassId, string userId);

        Task BookAsync(string id, string userId);

        Task UnBookAsync(string fitnessClassId, string userId);

        //Task<IEnumerable<HouseServiceModel>> GetUnApprovedAsync();

        //Task ApproveHouseAsync(int houseId);

        Task<FitnessClass> GetByIdAsync(string fitnessClassId);

        Task CancelClassAsync(string fitnessClassId);

    }
}
