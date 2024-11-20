using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Data.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public Guid FitnessClassId { get; set; }

        [Required]
        [ForeignKey(nameof(FitnessClassId))]
        public FitnessClass FitnessClass { get; set; } = null!;

        [Required]
        public DateTime BookingDate { get; set; }
    }
}
