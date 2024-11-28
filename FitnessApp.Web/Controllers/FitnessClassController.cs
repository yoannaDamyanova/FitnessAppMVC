using FitnessApp.Data.Models;
using FitnessApp.Data.Repository;
using FitnessApp.Services.Data.Contracts;
using FitnessApp.Web.Attributes;
using FitnessApp.Web.Extensions;
using FitnessApp.Web.ViewModels.FitnessClass;
using FitnessApp.Web.ViewModels.Instructor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FitnessApp.Common.EntityValidationConstants;
using static FitnessApp.Web.ViewModels.FitnessClass.Extensions.ModelExtensions;

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

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> All([FromQuery] AllFitnessClassQueryModel model)
        {
            var classes = await fitnessService.AllAsync(
                model.Category,
                model.Status,
                model.SearchTerm,
                model.Sorting,
                model.CurrentPage,
                model.FitnessClassesPerPage);

            model.TotalFitnessClassesCount = classes.TotalClassesCount;

            model.FitnessClasses = classes.FitnessClasses;

            model.Categories = await fitnessService.AllCategoriesNamesAsync();

            model.Statuses = await fitnessService.AllStatusesNamesAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Booked()
        {
            var userId = User.Id();
            IEnumerable<FitnessClassServiceModel> model;

            if (User.IsAdmin())
            {
                return RedirectToAction("Booked", "FitnessClass", new { area = "Admin" });
            }

            model = await fitnessService.AllBookedByUserId(userId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyClasses()
        {
            var userId = User.Id();
            IEnumerable<FitnessClassInstructorViewModel> model;

            if (await instructorService.ExistsByUserIdAsync(userId) == false)
            {
                return Unauthorized();
            }

            int instructorId = await instructorService.GetInstructorByIdAsync(userId) ?? 0;

            model = await fitnessService.AllFitnessClassesByInstructorIdAsync(instructorId);

            var instructorModel = new MyClassesInstructorViewModel
            {
                Classes = model,
                TotalClassesCount = model.Count(),
                Rating = await instructorService.GetRatingByIdAsync(userId.ToString()),
            };

            return View(instructorModel);
        }

        [HttpPost]
        public async Task<IActionResult> Book(string fitnessClassId)
        {
            if (await fitnessService.ExistsAsync(fitnessClassId) == false)
            {
                return BadRequest();
            }

            if (await instructorService.ExistsByUserIdAsync(User.Id()) && User.IsAdmin() == false)
            {
                return Unauthorized();
            }

            await fitnessService.BookAsync(fitnessClassId, User.Id());

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> UnBook(string fitnessClassId)
        {
            if (await fitnessService.ExistsAsync(fitnessClassId) == false)
            {
                return BadRequest();
            }

            try
            {
                await fitnessService.UnBookAsync(fitnessClassId, User.Id());
            }
            catch (UnauthorizedAccessException exception)
            {
                return Unauthorized();
            }

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        [MustBeInstructor]
        public async Task<IActionResult> Delete(string fitnessClassId)
        {
            var fitnessClass = await fitnessService.GetByIdAsync(fitnessClassId);
            var categories = await fitnessService.AllCategoriesAsync();
            var category = categories.FirstOrDefault(c => c.Id == fitnessClass.CategoryId);

            var model = new FitnessClassDeleteViewModel
            {
                Id = fitnessClassId.ToString(),
                Title = fitnessClass.Title,
                CategoryName = category.Name,
                StartTime = fitnessClass.StartTime.ToString("dd/MM/yyyy HH:mm")
            };

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(FitnessClassDeleteViewModel model)
        {
            await fitnessService.DeleteAsync(model.Id);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [MustBeInstructor]
        public async Task<IActionResult> Edit(string fitnessClassId)
        {
            if (await fitnessService.ExistsAsync(fitnessClassId) == false)
            {
                return BadRequest();
            }

            var model = await fitnessService.GetFitnessClassFormModelByIdAsync(fitnessClassId);

            model.Categories = await fitnessService.AllCategoriesAsync();

            return View(model);
        }

        [HttpPost]
        [MustBeInstructor]
        public async Task<IActionResult> Edit(FitnessClassFormModel model)
        {
            if (await fitnessService.ExistsAsync(model.Id) == false)
            {
                return BadRequest();
            }

            if (await fitnessService.HasInstructorWithIdAsync(model.Id, User.Id()) == false
                && User.IsAdmin() == false)
            {
                return Unauthorized();
            }

            if (await fitnessService.CategoryExistsAsync(model.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await fitnessService.AllCategoriesAsync();

                return View(model);
            }

            await fitnessService.EditAsync(model);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string fitnessClassId)
        {
            if (await fitnessService.ExistsAsync(fitnessClassId) == false)
            {
                return BadRequest();
            }

            var model = await fitnessService.FitnessClassDetailsByIdAsync(fitnessClassId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> BookedClasses()
        {
            var bookedClasses = await fitnessService.AllBookedByUserId(User.Id());
            return View(bookedClasses);
        }

        [HttpGet]
        [MustBeInstructor]
        public async Task<IActionResult> CancelClass(string fitnessClassId)
        {
            var fitnessClass = await fitnessService.GetByIdAsync(fitnessClassId);

            var model = new FitnessClassCancelViewModel
            {
                Id = fitnessClassId,
                Title = fitnessClass.Title,
                StartTime = fitnessClass.StartTime.ToString()
            };

            return View(model);
        }

        [HttpPost]
        [MustBeInstructor]
        public async Task<IActionResult> CancelClass(FitnessClassCancelViewModel model)
        {
            await fitnessService.CancelClassAsync(model.Id);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> ReviewClass(string fitnessClassId)
        {
            var fitnessClass = await fitnessService.GetByIdAsync(fitnessClassId);

            var model = new FitnessClassReviewFormModel
            {
                FitnessClassId = fitnessClassId,
                ClassTitle = fitnessClass.Title,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ReviewClass(FitnessClassReviewFormModel model)
        {
            await fitnessService.WriteReviewAsync(model, User.Id());

            return RedirectToAction(nameof(All));
        }
    }
}
