using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace StudentRegistrationBureauMVC.ActionFilters
{
    public class AnonymousAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity!.IsAuthenticated == true)
            {
                // User is authenticated, return them to home page
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
