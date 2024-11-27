using FitnessApp.Web.ViewModels.FitnessClass;

namespace FitnessApp.Web.ViewModels.Instructor
{
    public class InstructorViewModel
    {
        public string FullName { get; set; } = string.Empty;

        public double Rating { get; set; } 

        public string Biography { get; set; } = string.Empty;

        public string Specializations { get; set; } = string.Empty;

        public IEnumerable<FitnessClassServiceModel>? Classes { get; set; } 
    }
}
