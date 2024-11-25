using System.ComponentModel.DataAnnotations;
using static FitnessApp.Common.ErrorMessageConstants;
using static FitnessApp.Common.EntityValidationConstants.Review;

namespace FitnessApp.Web.ViewModels.FitnessClass
{
    public class FitnessClassReviewFormModel
    {
        public string FitnessClassId { get; set; } = null!;

        public string ClassTitle { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(MaxCommentsLength,
            MinimumLength = 10,
            ErrorMessage = LengthMessage)]
        public string Comments { get; set; } = null!;
    }
}
