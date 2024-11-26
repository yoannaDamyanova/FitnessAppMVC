using FitnessApp.Services.Data;
using FitnessApp.Services.Data.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FitnessApp.Web.Controllers
{
    public class LicenseController : Controller
    {
        private readonly ILicenseGeneratorService _licenseGeneratorService;

        public LicenseController(ILicenseGeneratorService licenseGeneratorService)
        {
            _licenseGeneratorService = new LicenseGeneratorService();
        }
    }
}
