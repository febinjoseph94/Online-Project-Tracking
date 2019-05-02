using OnlineProjectTracking.DAL;
using OnlineProjectTracking.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace OnlineProjectTracking.Controllers
{
    [CustomActionFilter]
    public class UserStoriesController : Controller
    {
        // GET: UserStories
        public ActionResult Index()
        {
            OPTDBContext context = new OPTDBContext();
            List<UserStory> stories = context.UserStories.ToList();
            return View(stories);
        }

        [HttpGet]
        public ActionResult AddNewStory()
        {
            ViewBag.Projects = GetProjectList();
            return View();
        }

        [HttpPost]
        public ActionResult AddNewStory(UserStory userStory)
        {
            try
            {
                ViewBag.Projects = GetProjectList();
                if (!ModelState.IsValid)
                    return View();
                else
                {
                    OPTDBContext dbEntities = new OPTDBContext();
                    dbEntities.UserStories.Add(userStory);
                    dbEntities.SaveChanges();
                    TempData["newstory_success"] = "added";
                }
            }
            catch (System.Exception)
            {
                TempData["newstory_error"] = "error";
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditStory(int? id)
        {
            OPTDBContext dbEntities = new OPTDBContext();
            UserStory story = dbEntities.UserStories.Find(id);
            List<SelectListItem> projects = GetProjectList();
            projects.Find(p => p.Value == story.ProjectID.ToString()).Selected = true;
            ViewBag.Projects = projects;
            return View(story);
        }

        [HttpPost]
        public ActionResult EditStory(UserStory userStory)
        {
            List<SelectListItem> projects = GetProjectList();
            projects.Find(p => p.Value == userStory.ProjectID.ToString()).Selected = true;
            ViewBag.Projects = projects;
            try
            {                
                if (!ModelState.IsValid)
                    return View();
                else
                {
                    using (OPTDBContext dbEntities = new OPTDBContext())
                    {
                        dbEntities.UserStories.Add(userStory);
                        dbEntities.Entry(userStory).State = System.Data.Entity.EntityState.Modified;
                        dbEntities.SaveChanges();
                    }
                    TempData["story_eidt_success"] = "sucess";
                    return View(userStory);
                }
            }
            catch (System.Exception)
            {
                TempData["story_eidt_error"] = "failed.";
                return View(userStory);
            }
        }


        private List<SelectListItem> GetProjectList()
        {
            List<SelectListItem> projects = new List<SelectListItem>() { new SelectListItem { Text = "Please select", Value = "", Disabled = true, Selected = true } };
            OPTDBContext dbEntities = new OPTDBContext();
            List<Project> dbprojects = dbEntities.Projects.ToList();
            foreach (Project project in dbprojects)
            {
                projects.Add(new SelectListItem()
                {
                    Text = project.ProjectName,
                    Value = project.ProjectID.ToString()
                });
            }
            return projects;
        }

    }
}