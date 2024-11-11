using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.Services.Data.Contracts;
using FitnessApp.Web.Extensions;
using FitnessApp.Web.Controllers;

namespace FitnessApp.Web.Attributes
{
    public class MustBeAgentAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            IInstructorService? agentService = context.HttpContext.RequestServices.GetService<IInstructorService>();

            if (agentService == null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (agentService != null
                && agentService.ExistsByIdAsync(context.HttpContext.User.Id()).Result == false)
            {
                context.Result = new RedirectToActionResult(nameof(InstructorController.Become), "Agent", null);
            }
        }
    }
}
