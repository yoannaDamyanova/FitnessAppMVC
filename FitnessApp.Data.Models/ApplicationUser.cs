using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static FitnessApp.Common.EntityValidationConstants.User;

namespace FitnessApp.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(MaxNameLength)]
        [PersonalData]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(MaxNameLength)]
        [PersonalData]
        public string LastName { get; set; } = null!;
    }
}
