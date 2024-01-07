using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace StudentRegistrationBureauMVC.ActionFilters
{
    public class AuthenticatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity!.IsAuthenticated == false)
            {
                // User is not authenticated, return a view with a message
                context.Result = new RedirectToActionResult("UnauthenticatedError", "Auth", null);
            }
        }
    }
}
