using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Web.ViewModels.Instructor
{
    public class InstructorRateFormModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        public double Rating { get; set; }
    }
}
