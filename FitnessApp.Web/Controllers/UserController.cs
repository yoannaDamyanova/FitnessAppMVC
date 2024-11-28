using FitnessApp.Services.Data.Contracts;
using FitnessApp.Web.ViewModels.Instructor;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IFitnessClassService fitnessService;

        private readonly IInstructorService instructorService;

        public UserController(IFitnessClassService fitnessService, IInstructorService instructorService)
        {
            this.fitnessService = fitnessService;
            this.instructorService = instructorService;
        }

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

        public async Task<IActionResult> RateInstructor(InstructorRateFormModel model, int instructorId)
        {
            await instructorService.
        }
    }
}
