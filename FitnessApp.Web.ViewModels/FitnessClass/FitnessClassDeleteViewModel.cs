using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Web.ViewModels.FitnessClass
{
    public class FitnessClassDeleteViewModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public string StartTime { get; set; } = null!;
    }
}
