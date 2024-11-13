using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static FitnessApp.Common.EntityValidationConstants.FitnessClass;

namespace FitnessApp.Data.Models
{
    public class FitnessClass
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(MaxTitleLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int InstructorId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        [Range(MinCapacity, MaxCapacity)]
        public int Capacity { get; set; }

        [Required]
        public bool Status { get; set; } 

        // Navigation properties
        [Required]
        [ForeignKey(nameof(InstructorId))]
        public virtual Instructor Instructor { get; set; } = null!;

        [Required]
        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    }
}
