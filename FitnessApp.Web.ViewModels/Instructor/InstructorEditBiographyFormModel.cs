using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static FitnessApp.Common.EntityValidationConstants.Instructor;
using static FitnessApp.Common.ErrorMessageConstants;

namespace FitnessApp.Web.ViewModels.Instructor
{
    public class InstructorEditBiographyFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(MaxBiographyLength,
            MinimumLength = MinBiographyLength,
            ErrorMessage = LengthMessage)]
        public string Biography { get; set; } = null!;
    }
}
