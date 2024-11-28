using System.ComponentModel.DataAnnotations;
using static FitnessApp.Common.EntityValidationConstants.Instructor;
using static FitnessApp.Common.ErrorMessageConstants;


namespace FitnessApp.Web.ViewModels.Instructor
{
    public class InstructorEditSpecializationsFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(MaxSpecializationsLength,
            MinimumLength = MinSpecializationsLength,
            ErrorMessage = LengthMessage)]
        public string Specializations { get; set; } = null!;
    }
}
