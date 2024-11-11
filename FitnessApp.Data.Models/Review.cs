using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Data.Models
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public Guid FitnessClassId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength()]
        public string Comments { get; set; } = null!;

        [Required]
        public DateTime DateSubmitted { get; set; }

        // Navigation properties
        [Required]
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(FitnessClassId))]
        public virtual FitnessClass FitnessClass { get; set; } = null!;
    }
}
