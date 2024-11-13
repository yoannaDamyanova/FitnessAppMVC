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

        Task<bool> CategoryExistsAsync(int categoryId);

        Task<Guid> AddClassAsync(FitnessClassFormModel model, int instructorId);

        //Task<HouseQueryServiceModel> AllAsync(
        //    string? category = null,
        //    string? searchTerm = null,
        //    HouseSorting sorting = HouseSorting.Newest,
        //    int currentPage = 1,
        //    int housesPerPage = 1);

        //Task<IEnumerable<string>> AllCategoriesNamesAsync();

        //Task<IEnumerable<HouseServiceModel>> AllHousesByAgentIdAsync(int agentId);

        //Task<IEnumerable<HouseServiceModel>> AllHousesByUserId(string userId);

        //Task<bool> ExistsAsync(int id);

        //Task<HouseDetailsServiceModel> HouseDetailsByIdAsync(int id);

        //Task EditAsync(int houseId, HouseFormModel model);

        //Task<bool> HasAgentWithIdAsync(int houseId, string userId);

        //Task<HouseFormModel?> GetHouseFormModelByIdAsync(int id);

        //Task DeleteAsync(int houseId);

        //Task<bool> IsRentedAsync(int houseId);

        //Task<bool> IsRentedByIUserWithIdAsync(int houseId, string userId);

        //Task RentAsync(int id, string userId);

        //Task LeaveAsync(int houseId, string userId);

        //Task<IEnumerable<HouseServiceModel>> GetUnApprovedAsync();

        //Task ApproveHouseAsync(int houseId);
    }
}
