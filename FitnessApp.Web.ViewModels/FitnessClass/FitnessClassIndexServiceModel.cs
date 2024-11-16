using FitnessApp.Web.ViewModels.FitnessClass.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Web.ViewModels.FitnessClass
{
    public class FitnessClassIndexServiceModel : IFitnessClassModel
    {
        public string Id { get; set; }
        public string Title { get ; set ; }

        public string Description { get ; set ; }

        public string StartTime { get; set; }

        public string InstructorName { get; set; }

        public string Duration { get; set; }
        public bool IsActive { get ; set ; }
    }
}
