using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Data.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<FitnessClass> Classes { get; set; } = new HashSet<FitnessClass>();
    }
}
