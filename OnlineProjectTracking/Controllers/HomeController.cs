using OnlineProjectTracking.Security;
using System.Web.Mvc;
namespace OnlineProjectTracking.Controllers
{
    [CustomActionFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}