using OnlineProjectTracking.DAL;
using OnlineProjectTracking.ViewModels;
using System.Linq;
using System.Web.Mvc;
namespace OnlineProjectTracking.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["login_error"] = "Please fill all the fields";
                return View(model);
            }
            else
            {
                OPTDBContext dbEntities = new OPTDBContext();
                UserAccount user = dbEntities.UserAccounts.Where(account => account.Email == model.email && account.Password == model.password).SingleOrDefault();
                if (user != null)
                {
                    Session["user_id"] = user.EmployeeID;
                    Session["user_email"] = user.Email;
                    Session["name"] = user.Employee.EmployeeName;
                    Session["role"] = user.Employee.Designation;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["invalid_login"] = "Please try with valid credentials";
                    return View(model);
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}