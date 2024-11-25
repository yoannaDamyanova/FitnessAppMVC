using FitnessApp.Web.ViewModels.FitnessClass.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Web.ViewModels.FitnessClass
{
    public class FitnessClassInstructorViewModel : IFitnessClassModel
    {
        public string FitnessClassId { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Status { get; set; } = null!;

        public int LeftCapacity {  get; set; }

        public string StartTime { get; set; } = null!;
    }
}
