
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DubaiChaRaja.Filters
{
    public class RequireSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            if (session.GetInt32("UserId") == null)
            {
                context.Result = new UnauthorizedObjectResult("Session expired or user not logged in.");
            }
        }
    }
}