using FitnessApp.Web.ViewModels.FitnessClass.Contracts;
using System.ComponentModel.DataAnnotations;
using static FitnessApp.Common.ErrorMessageConstants;
using static FitnessApp.Common.EntityValidationConstants.FitnessClass;
using FitnessApp.Web.ViewModels.Attributes;
using static FitnessApp.Common.EntityValidationConstants;

namespace FitnessApp.Web.ViewModels.FitnessClass
{
    public class FitnessClassServiceModel : IFitnessClassModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(MaxTitleLength,
            MinimumLength = MinTitleLength,
            ErrorMessage = LengthMessage)]
        public string Title { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [Range(MinDurationMinutes, MaxDurationMinutes)]
        public int Duration { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [Range(MinCapacity, MaxCapacity)]
        public int Capacity { get; set; }

        [DateTimeFormat(DateFormat)]
        public string StartTime { get; set; } = string.Empty;

        public string InstructorFullName { get; set; } = string.Empty;
    }
}
