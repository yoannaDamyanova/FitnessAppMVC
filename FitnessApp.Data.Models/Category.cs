using System.ComponentModel.DataAnnotations;
using static FitnessApp.Common.EntityValidationConstants.Category;

namespace FitnessApp.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxNameLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<FitnessClass> Classes { get; set; } = new HashSet<FitnessClass>();
    }
}
