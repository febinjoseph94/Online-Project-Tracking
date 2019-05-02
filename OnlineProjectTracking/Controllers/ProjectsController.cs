using OnlineProjectTracking.DAL;
using OnlineProjectTracking.Security;
using OnlineProjectTracking.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace OnlineProjectTracking.Controllers
{
    [CustomActionFilter]
    public class ProjectsController : Controller
    {
        // GET: Projects
        public ActionResult Index()
        {
            OPTDBContext context = new OPTDBContext();
            List<Project> projects = context.Projects.ToList();
            return View(projects);
        }

        /// add new project
        public ActionResult AddNewProject()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewProject(ProjectViewModel project)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
                else
                {
                    OPTDBContext dbEntities = new OPTDBContext();
                    Project dbproject = new Project()
                    {
                        ProjectName = project.ProjectName,
                        ClientName = project.ClientName,
                        StartDate = project.StartDate,
                        EndDate = project.EndDate,
                        ProjectDescription = project.ProjectDescription
                    };
                    dbEntities.Projects.Add(dbproject);
                    dbEntities.SaveChanges();
                    TempData["newproject_success"] = "added";
                    return View();
                }
            }
            catch (System.Exception)
            {
                TempData["newproject_error"] = "error";
            }
            return View();
        }

        public ActionResult EditProject(int? id)
        {
            OPTDBContext dbEntities = new OPTDBContext();
            Project project = dbEntities.Projects.Find(id);
            return View(project);
        }
        [HttpPost]
        public ActionResult EditProject(Project project)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
                else
                {
                    using (OPTDBContext dbEntities = new OPTDBContext())
                    {
                        dbEntities.Projects.Add(project);
                        dbEntities.Entry(project).State = System.Data.Entity.EntityState.Modified;
                        dbEntities.SaveChanges();
                    }
                    TempData["project_eidt_success"] = "Project updated successfully.";
                    return View(project);
                }
            }
            catch (System.Exception)
            {
                TempData["project_eidt_error"] = "Project record update failed.";
                return View(project);
            }
        }
    }
}