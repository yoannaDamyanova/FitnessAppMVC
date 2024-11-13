
using FitnessApp.Services.Data.Contracts;
using FitnessApp.Web.Attributes;
using FitnessApp.Web.Extensions;
using FitnessApp.Web.ViewModels.FitnessClass;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Web.Controllers
{
    public class FitnessClassController : BaseController
    {
        private readonly IFitnessClassService fitnessService;

        private readonly IInstructorService instructorService;

        public FitnessClassController(IFitnessClassService fitnessService, IInstructorService instructorService)
        {
            this.fitnessService = fitnessService;
            this.instructorService = instructorService;
        }

        public void Index()
        {

        }

        [HttpGet]
        [MustBeInstructor]
        public async Task<IActionResult> Add()
        {
            var model = new FitnessClassFormModel()
            {
                Categories = await fitnessService.AllCategoriesAsync(),
            };

            return View(model);
        }

        [HttpPost]
        [MustBeInstructor]
        public async Task<IActionResult> Add(FitnessClassFormModel model)
        {
            if (await fitnessService.CategoryExistsAsync(model.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await fitnessService.AllCategoriesAsync();

                return View(model);
            }

            int? instructorId = await instructorService.GetInstructorByIdAsync(User.Id());

            Guid fitnessClassId = await fitnessService.AddClassAsync(model, instructorId ?? 0);

            return RedirectToAction(nameof(HomeController.Index));
        }
    }
}
