using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Web.ViewModels.FitnessClass
{
    public class FitnessClassQueryServiceModel
    {
        public int TotalClassesCount { get; set; }

        public IEnumerable<FitnessClassServiceModel> FitnessClasses { get; set; } = new List<FitnessClassServiceModel>();
    }
}
