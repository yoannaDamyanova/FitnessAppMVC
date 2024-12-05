using FitnessApp.Services.Data.Contracts;
using FitnessApp.Web.Attributes;
using FitnessApp.Web.Extensions;
using FitnessApp.Web.ViewModels.Instructor;
using Microsoft.AspNetCore.Authorization;
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
            if (await instructorService.UserWithLicenseNumberExistsInDbAsync(model.LicenseNumber))
            {
                ModelState.AddModelError(nameof(model.LicenseNumber), LicenseNumberExistsInDb);
            }

            if(!instructorService.UserWithLicenseNumberExistsGlobally(model.LicenseNumber))
            {
                ModelState.AddModelError(nameof(model.LicenseNumber), LicenseNumberIsNotDiscoverable);
            }

            if (!await instructorService.IsLicenseNumberValidAsync(model.LicenseNumber))
            {
                ModelState.AddModelError(nameof(model.LicenseNumber), LicenseNumberIsNotValid);
            }

            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await instructorService.CreateAsync(model, User.Id());

            return RedirectToAction("All", "FitnessClass"); 
        }

        [HttpGet]
        public async Task<IActionResult> InstructorShowCase(int instructorId)
        {
            if (await instructorService.ExistsByIdAsync(instructorId) == false)
            {
                return BadRequest();
            }

            var model = await instructorService.GetInstructorViewModelByIdAsync(instructorId);

            return View(model);
        }

        [HttpGet]
        [Authorize]
        [MustBeInstructor]
        public async Task<IActionResult> EditBiography(int instructorId)
        {
            if (await instructorService.ExistsByIdAsync(instructorId) == false)
            {
                return BadRequest();
            }

            var instructor = await instructorService.GetByIdAsync(instructorId);

            var model = new InstructorEditBiographyFormModel()
            {
                Id = instructorId,
                Biography = instructor.Biography
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [MustBeInstructor]
        public async Task<IActionResult> EditBiography(int instructorId, InstructorEditBiographyFormModel model)
        {
            if (await instructorService.ExistsByIdAsync(instructorId) == false)
            {
                return BadRequest();
            }

            await instructorService.EditBiographyAsync(model, instructorId);

            return RedirectToAction("All", "FitnessClass");
        }

        [HttpGet]
        [Authorize]
        [MustBeInstructor]
        public async Task<IActionResult> EditSpecializations(int instructorId)
        {
            if (await instructorService.ExistsByIdAsync(instructorId) == false)
            {
                return BadRequest();
            }

            var instructor = await instructorService.GetByIdAsync(instructorId);

            var model = new InstructorEditSpecializationsFormModel()
            {
                Id = instructorId,
                Specializations = instructor.Specializations
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [MustBeInstructor]
        public async Task<IActionResult> EditSpecializations(int instructorId, InstructorEditSpecializationsFormModel model)
        {
            if (await instructorService.ExistsByIdAsync(instructorId) == false)
            {
                return BadRequest();
            }

            await instructorService.EditSpecializationsAsync(model, instructorId);

            return RedirectToAction(nameof(InstructorShowCase), nameof(InstructorController), instructorId);
        }
    }
}
