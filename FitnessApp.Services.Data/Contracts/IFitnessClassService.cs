﻿using FitnessApp.Data.Models;
using FitnessApp.Services.Data.Enumerators;
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

        Task<FitnessClassQueryServiceModel> AllAsync(
            string? category = null,
            string? searchTerm = null,
            FitnessClassSorting sorting = FitnessClassSorting.Newest,
            int currentPage = 1,
            int housesPerPage = 1);

        Task<IEnumerable<string>> AllCategoriesNamesAsync();

        Task<IEnumerable<FitnessClassServiceModel>> AllFitnessClassesByInstructorIdAsync(int agentId);

        Task<IEnumerable<FitnessClassServiceModel>> AllBookedByUserId(string userId);

        Task<bool> ExistsAsync(string id);

        //Task<HouseDetailsServiceModel> HouseDetailsByIdAsync(int id);

        //Task EditAsync(int houseId, HouseFormModel model);

        Task<bool> HasInstructorWithIdAsync(int fitnessClassId, string userId);

        Task<FitnessClassFormModel?> GetFitnessClassFormModelByIdAsync(string id);

        //Task DeleteAsync(int houseId);

        //Task<bool> IsRentedByIUserWithIdAsync(int houseId, string userId);

        //Task RentAsync(int id, string userId);

        //Task LeaveAsync(int houseId, string userId);

        //Task<IEnumerable<HouseServiceModel>> GetUnApprovedAsync();

        //Task ApproveHouseAsync(int houseId);
    }
}
