using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Data.Models
{
    [PrimaryKey(nameof(UserId), nameof(FitnessClassId))]
    public class Booking
    {
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
