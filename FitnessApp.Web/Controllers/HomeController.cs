using FitnessApp.Services.Data.Contracts;
using FitnessApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FitnessApp.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFitnessClassService fitnessService;

        public HomeController(
            ILogger<HomeController> logger,
            IFitnessClassService _fitnessService)
        {
            _logger = logger;
            fitnessService = _fitnessService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var model = await fitnessService.LastFiveHousesAsync();

            return View(model);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {

            if (statusCode == 400)
            {
                return View("Error400");
            }

            if (statusCode == 401)
            {
                return View("Error401");
            }

            if (statusCode == 404)
            {
                return View("Error404");
            }

            if (statusCode == 500)
            {
                return View("Error500");
            }
            return View();
        }
    }
}
