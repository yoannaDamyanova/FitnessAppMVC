using FitnessApp.Web.ViewModels.Attributes;
using FitnessApp.Web.ViewModels.Category;
using FitnessApp.Web.ViewModels.FitnessClass.Contracts;
using System.ComponentModel.DataAnnotations;
using static FitnessApp.Common.EntityValidationConstants.FitnessClass;
using static FitnessApp.Common.ErrorMessageConstants;

namespace FitnessApp.Web.ViewModels.FitnessClass
{
    public class FitnessClassFormModel : IFitnessClassModel
    {
        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(MaxTitleLength,
            MinimumLength = MinTitleLength,
            ErrorMessage = LengthMessage)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(MaxTitleLength,
            MinimumLength = MinTitleLength,
            ErrorMessage = LengthMessage)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [DateTimeFormat("dd/MM/yyyy HH:mm")] // custom attribute
        public string StartTime { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        [Range(MinDurationMinutes, MaxDurationMinutes)]
        public int Duration { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [Range(MinCapacity, MaxCapacity)]
        public int Capacity { get; set; }

        public IEnumerable<FitnessClassCategoryServiceModel> Categories { get; set; } =
            new HashSet<FitnessClassCategoryServiceModel>();

        public string Status { get; set; } = null!;

        public string? Id { get; set; }
    }
}
