using FitnessApp.Web.ViewModels.FitnessClass.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FitnessApp.Web.ViewModels.FitnessClass.Extensions
{
    public static class ModelExtensions
    {
        public static string GetInformation(IFitnessClassModel fitnessClass)
        {
            string information = fitnessClass.Title + GetStatus(fitnessClass.IsActive);
            information = Regex.Replace(information, @"[^a-zA-Z0-9\-]", string.Empty);

            return information;
        }

        public static string GetStatus(bool status)
        {
            return status == true ? "Active" : "Canceled";
        }
    }
}
