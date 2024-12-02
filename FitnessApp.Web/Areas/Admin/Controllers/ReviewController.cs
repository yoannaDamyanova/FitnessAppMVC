using FitnessApp.Services.Data.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Web.Areas.Adminn.Controllers
{
    public class ReviewController : AdminBaseController
    {
        private readonly IFitnessClassService fitnessClassService;

        public ReviewController(IFitnessClassService fitnessClassService)
        {
            this.fitnessClassService = fitnessClassService;
        }

        public async Task<IActionResult> All()
        {
            var model = await fitnessClassService.AllReviewsAsync();

            return View(model);
        }
    }
}
