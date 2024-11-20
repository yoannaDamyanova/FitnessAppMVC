using FitnessApp.Web.ViewModels.Instructor;

namespace FitnessApp.Web.ViewModels.FitnessClass
{
    public class FitnessClassDetailsServiceModel : FitnessClassServiceModel
    {
        public string Description { get; set; } = null!;
         
        public string Category { get; set; } = null!;

        public InstructorServiceModel Instructor { get; set; } = null!;
    }
}
