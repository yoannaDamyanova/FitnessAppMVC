using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static FitnessApp.Common.EntityValidationConstants.User;

namespace FitnessApp.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(MaxNameLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(MaxNameLength)]
        public string LastName { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
