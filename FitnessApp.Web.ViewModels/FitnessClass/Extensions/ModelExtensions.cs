using FitnessApp.Web.ViewModels.FitnessClass.Contracts;
using System.Text.RegularExpressions;

namespace FitnessApp.Web.ViewModels.FitnessClass.Extensions
{
    public static class ModelExtensions
    {
        public static string GetInformation(IFitnessClassModel fitnessClass)
        {
            string information = fitnessClass.Title + fitnessClass.Status;
            information = Regex.Replace(information, @"[^a-zA-Z0-9\-]", string.Empty);

            return information;
        }
    }
}
