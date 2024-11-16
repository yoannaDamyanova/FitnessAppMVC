using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Web.ViewModels.FitnessClass
{
    public class FitnessClassDetailsServiceModel : FitnessClassServiceModel
    {
        public string Description { get; set; }

        public string Category { get; set; }
    }
}
