using FitnessApp.Services.Data.Contracts;
using FitnessApp.Web.Attributes;
using FitnessApp.Web.Extensions;
using FitnessApp.Web.ViewModels.Instructor;
using Microsoft.AspNetCore.Mvc;
using static FitnessApp.Common.ErrorMessageConstants;

namespace FitnessApp.Web.Controllers
{
    public class InstructorController : BaseController
    {
        private readonly IInstructorService instructorService;

        public InstructorController(IInstructorService _instructorService)
        {
            instructorService = _instructorService;
        }

        [HttpGet]
        [NotAnInstructor]
        public IActionResult Become()
        {
            var model = new BecomeInstructorFormModel();

            return View(model);
        }

        [HttpPost]
        [NotAnInstructor]
        public async Task<IActionResult> Become(BecomeInstructorFormModel model)
        {
            if (await instructorService.UserWithLicenseNumberExistsAsync(model.LicenseNumber))
            {
                ModelState.AddModelError(nameof(model.LicenseNumber), LicenseNumberExists);
            }

            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await instructorService.CreateAsync(User.Id());

            return RedirectToAction(nameof(FitnessClassController.Index), "FitnessClass"); 
        }
    }
}
