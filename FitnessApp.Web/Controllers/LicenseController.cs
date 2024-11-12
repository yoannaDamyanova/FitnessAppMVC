using FitnessApp.Services.Data;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Web.Controllers
{
    public class LicenseController : Controller
    {
        private readonly LicenseGeneratorService _licenseGeneratorService;

        public LicenseController()
        {
            _licenseGeneratorService = new LicenseGeneratorService();
        }

        public IActionResult GenerateLicenseNumbers()
        {
            _licenseGeneratorService.GenerateLicenseNumbers();
            return Content("License numbers generated successfully.");
        }
    }
}
