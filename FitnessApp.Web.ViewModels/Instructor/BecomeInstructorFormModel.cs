using static FitnessApp.Common.ErrorMessageConstants;
using static FitnessApp.Common.EntityValidationConstants.Instructor;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Web.ViewModels.Instructor
{
    public class BecomeInstructorFormModel
    {
        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(MaxBiographyLength, 
            MinimumLength = MinBiographyLength, 
            ErrorMessage = LengthMessage)]
        public string Biography { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(MaxSpecializationsLength, 
            MinimumLength = MinSpecializationsLength, 
            ErrorMessage = LengthMessage)]
        public string Specializations { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(MaxLicenseNumberLenght, MinimumLength = MinLicenseNumberLenght, ErrorMessage = LengthMessage)]
        public int LicenseNumber { get; set; }
    }
}
