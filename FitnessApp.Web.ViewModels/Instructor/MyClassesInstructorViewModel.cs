using FitnessApp.Web.ViewModels.FitnessClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Web.ViewModels.Instructor
{
    public class MyClassesInstructorViewModel : InstructorServiceModel
    {
        public int TotalClassesCount { get; set; }

        public IEnumerable<FitnessClassInstructorViewModel> Classes { get; set; } = new List<FitnessClassInstructorViewModel>();
    }
}
