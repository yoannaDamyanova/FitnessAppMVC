using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Web.Controllers
{
    public class FitnessClassController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
