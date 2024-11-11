using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.Services.Data.Contracts;
using FitnessApp.Web.Extensions;

namespace FitnessApp.Web.Attributes
{
    public class NotAnInstructorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            IInstructorService? instructorService = context.HttpContext.RequestServices.GetService<IInstructorService>();

            if (instructorService == null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (instructorService != null
                && instructorService.ExistsByIdAsync(context.HttpContext.User.Id()).Result)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
        }
    }
}
