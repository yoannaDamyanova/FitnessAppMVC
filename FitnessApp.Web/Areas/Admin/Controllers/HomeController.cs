using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        public IActionResult DashBoard()
        {
            return View();
        }

        public async Task<IActionResult> ForReview()
        {
            return View();
        }
    }
}
