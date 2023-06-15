using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NexusApp.Areas.Financial
{
    public class CustomAuthorizationAttribute : ActionFilterAttribute
    {
        public string?[] Roles { get; set; }
        public CustomAuthorizationAttribute(params string[] roles)
        {
            Roles = roles;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool isAuthrized = false;
            var rolec = context.HttpContext.Session.GetString("Role");
            if (rolec != null)
            {
                foreach (string role in Roles)
                {
                    if (role.Equals(rolec))
                    {
                        isAuthrized = true;
                        break;
                    }
                }
                if (!isAuthrized)
                {
                    string currentPath = context.HttpContext.Request.Path.ToString();
                    context.Result = new RedirectToActionResult("Index", "Login", new { returnUrl = currentPath });
                    context.Result = new ViewResult
                    {
                        ViewName = "~/Views/Login/Index.cshtml" 
                    };


                }
            }
            else
            {

                string currentPath = context.HttpContext.Request.Path.ToString();
                context.Result = new RedirectToActionResult("Index", "Login", new { returnUrl = currentPath });
                context.Result = new ViewResult
                {
                    ViewName = "~/Views/Login/Index.cshtml"
                };

            }
        }
    }
}
