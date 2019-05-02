using System.Web.Mvc;
namespace OnlineProjectTracking.Security
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["user_email"] != null)
                base.OnActionExecuting(filterContext);
            else
                filterContext.Result = new RedirectResult("~/Account/Login");
        }
    }
}