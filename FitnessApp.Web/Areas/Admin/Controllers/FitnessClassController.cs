using FitnessApp.Services.Data.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Web.Areas.Admin.Controllers
{
    public class FitnessClassController : AdminBaseController
    {
        private readonly IFitnessClassService fitnessClassService;
        private readonly IInstructorService instructorService;

        public FitnessClassController(IFitnessClassService fitnessClassService, IInstructorService instructorService)
        {
            this.fitnessClassService = fitnessClassService;
            this.instructorService = instructorService;
        }

        [HttpGet]
        public async Task<IActionResult> Approve()
        {
            var model = await fitnessClassService.GetUnApprovedAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(string fitnessClassId)
        {
            Guid id = Guid.Empty;
            if (!IsGuidValid(fitnessClassId, ref id))
            {
                return BadRequest();
            }
            await fitnessClassService.ApproveFitnessClassAsync(id);

            return RedirectToAction(nameof(Approve));
        }
    }
}
