using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Web.ViewModels.FitnessClass.Contracts
{
    public interface IFitnessClassModel
    {
        public string Title { get; set; }

        public bool IsActive { get; set; }
    }
}
