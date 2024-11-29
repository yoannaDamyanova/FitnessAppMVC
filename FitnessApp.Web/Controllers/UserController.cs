using FitnessApp.Services.Data.Contracts;
using FitnessApp.Web.ViewModels.Instructor;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IInstructorService instructorService;

        public UserController(IInstructorService instructorService)
        {
            this.instructorService = instructorService;
        }


        [HttpGet]
        public async Task<IActionResult> RateInstructor(int instructorId)
        {
            var instructor = await instructorService.GetByIdAsync(instructorId);

            var model = new InstructorRateFormModel()
            {
                Id = instructor.Id,
                Rating = instructor.Rating,
                FullName = instructor.User.FirstName + " " + instructor.User.LastName,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RateInstructor(InstructorRateFormModel model)
        {
            await instructorService.Rate(model, model.Id);

            return RedirectToAction("All", "FitnessClass");
        }
    }
}
