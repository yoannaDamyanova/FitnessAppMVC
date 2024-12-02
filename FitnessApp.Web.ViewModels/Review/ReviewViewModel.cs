using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Web.ViewModels.Review
{
    public class ReviewViewModel
    {
        public string FitnessClassTitle { get; set; } = string.Empty;
        public int Rating { get; set; }

        public string Comments { get; set; } = string.Empty;

        public string ReviewerName {  get; set; } = string.Empty;

        public string InstructorFullName {  get; set; } = string.Empty;
    }
}
