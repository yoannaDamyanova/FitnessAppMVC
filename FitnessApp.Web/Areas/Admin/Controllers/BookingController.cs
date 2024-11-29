using FitnessApp.Services.Data.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Web.Areas.Admin.Controllers
{
    public class BookingController : AdminBaseController
    {
        private readonly IFitnessClassService fitnessClassService;

        public BookingController(IFitnessClassService fitnessClassService)
        {
            this.fitnessClassService = fitnessClassService;
        }

        public async Task<IActionResult> All()
        {
            var model = await fitnessClassService.AllBookingsAsync();

            return View(model);
        }
    }
}
