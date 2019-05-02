using System.Linq;
using System.Web.Mvc;
using OnlineProjectTracking.DAL;
using System.Collections.Generic;
namespace OnlineProjectTracking.Controllers
{
    public class ManagerCommentsController : Controller
    {

       
        [HttpGet]
        public ActionResult AddComment(int id)
        {
            ViewBag.TaskID = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddComment(ManagerComment managerComment)
        {
            ViewBag.TaskID = managerComment.ProjectTaskID;
            try
            {
                if (!ModelState.IsValid)
                    return View();
                else
                {
                    OPTDBContext dbEntities = new OPTDBContext();
                    dbEntities.ManagerComments.Add(managerComment);
                    dbEntities.SaveChanges();
                    TempData["newcomment_success"] = "added";
                    return View();
                }
            }
            catch (System.Exception)
            {
                TempData["newcomment_error"] = "error";
            }
            return View();
        }
    }
}