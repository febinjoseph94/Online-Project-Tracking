using OnlineProjectTracking.DAL;
using OnlineProjectTracking.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace OnlineProjectTracking.Controllers
{
    [CustomActionFilter]
    public class ProjectTasksController : Controller
    {
        // GET: ProjectTasks
        public ActionResult Index()
        {
            OPTDBContext context = new OPTDBContext();
            List<ProjectTask> projectTasks;
            if (!Session["role"].ToString().Equals("Developer") && !Session["role"].ToString().Equals("Designer"))
            {
                projectTasks = context.ProjectTasks.ToList();
            }
            else
            {
                int useID = int.Parse(Session["user_id"].ToString().Trim());
                projectTasks = context.ProjectTasks.Where(task => task.AssignedTo == useID).ToList();
            }
            return View(projectTasks);
        }


        [HttpGet]
        public ActionResult AddNewTask()
        {
            ViewBag.Employees = GetEmployeeList();
            ViewBag.UserStories = GetUserStoriesList();
            ViewBag.TaskTypes = GetTaskTypeList();
            return View();
        }

        [HttpPost]
        public ActionResult AddNewTask(ProjectTask projectTask)
        {
            ViewBag.Employees = GetEmployeeList();
            ViewBag.UserStories = GetUserStoriesList();
            ViewBag.TaskTypes = GetTaskTypeList();
            try
            {
                if (!ModelState.IsValid)
                    return View();

                OPTDBContext dbEntities = new OPTDBContext();
                dbEntities.ProjectTasks.Add(projectTask);
                dbEntities.SaveChanges();
                TempData["newtask_success"] = "added";
            }
            catch (System.Exception)
            {
                TempData["newtask_error"] = "error";
            }
            return View();
        }


        private List<SelectListItem> GetEmployeeList()
        {
            List<SelectListItem> employees = new List<SelectListItem>() { new SelectListItem { Text = "Please select", Value = "", Disabled = true, Selected = true } };
            OPTDBContext dbEntities = new OPTDBContext();
            List<Employee> dbemployees = dbEntities.Employees.Where(emp => emp.Designation != "Project Manager" && emp.Designation != "Business Analyst" && emp.Designation != "Admin").ToList();
            foreach (Employee employee in dbemployees)
            {
                employees.Add(new SelectListItem()
                {
                    Text = employee.EmployeeName,
                    Value = employee.EmployeeID.ToString()
                });
            }
            return employees;
        }


        private List<SelectListItem> GetUserStoriesList()
        {
            List<SelectListItem> userstories = new List<SelectListItem>() { new SelectListItem { Text = "Please select", Value = "", Disabled = true, Selected = true } };
            OPTDBContext dbEntities = new OPTDBContext();
            List<UserStory> dbuserstories = dbEntities.UserStories.ToList();
            foreach (UserStory userStory in dbuserstories)
            {
                userstories.Add(new SelectListItem()
                {
                    Text = userStory.Story,
                    Value = userStory.UserStoryID.ToString()
                });
            }
            return userstories;
        }

        private List<SelectListItem> GetTaskTypeList()
        {
            List<SelectListItem> taskTypeList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Please select", Value = "", Disabled = true, Selected = true },
                new SelectListItem {Text = "Technical Documentation", Value = "Technical Documentation" },
                new SelectListItem {Text = "Timeline", Value = "Timeline" },
                new SelectListItem {Text = "Code", Value = "Code" },
                new SelectListItem {Text = "User Documentation", Value = "User Documentation" },
                new SelectListItem {Text = "Miscellaneous", Value = "Miscellaneous" },
            };
            return taskTypeList;
        }


        [HttpPost]
        public JsonResult MarkCompleted(int task_id, int task_status)
        {
            try
            {
                OPTDBContext context = new OPTDBContext();
                ProjectTask projectTask = context.ProjectTasks.FirstOrDefault(pt => pt.ProjectTaskID == task_id);
                if (projectTask != null)
                {
                    projectTask.TaskStatus = task_status;
                    context.SaveChanges();
                    return Json(new { code = 200 });
                }
                return Json(new { code = 500 });
            }
            catch (System.Exception)
            {
                return Json(new { code = 200 });
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                OPTDBContext context = new OPTDBContext();
                ProjectTask projectTask = context.ProjectTasks.FirstOrDefault(pt => pt.ProjectTaskID == id);
                return View(projectTask);
            }
            catch (System.Exception)
            {
                return View(new ProjectTask());
            }
        }

        [HttpGet]
        public ActionResult UploadTask(int id)
        {
            try
            {
                OPTDBContext context = new OPTDBContext();
                ProjectTask projectTask = context.ProjectTasks.FirstOrDefault(pt => pt.ProjectTaskID == id);
                return View(projectTask);
            }
            catch (System.Exception)
            {
                return View(new ProjectTask());
            }
        }

        [HttpPost]
        public ActionResult UploadTask(ProjectTask projectTask)
        {
            try
            {
                OPTDBContext context = new OPTDBContext();
                ProjectTask projectTaskDB = context.ProjectTasks.FirstOrDefault(pt => pt.ProjectTaskID == projectTask.ProjectTaskID);
                projectTaskDB.TaskCompletionDesciption = string.IsNullOrEmpty(projectTask.TaskCompletionDesciption) ? "" : projectTask.TaskCompletionDesciption;
                HttpPostedFileBase file = Request.Files["file"];
                if (file != null && file.ContentLength > 0)
                {
                    string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-" + file.FileName;
                    string path = Server.MapPath("~/Content/Uploads/" + FileName);
                    file.SaveAs(path);
                    projectTaskDB.TaskData = FileName;
                }
                context.SaveChanges();
                TempData["data_upload_ok"] = "Ok";
                return View(projectTask);
            }
            catch (System.Exception ex)
            {
                TempData["data_upload_error"] = ex.ToString();
                return View(new ProjectTask());
            }
        }

        public FileResult DownloadTaskData(int id)
        {
            OPTDBContext context = new OPTDBContext();
            ProjectTask projectTaskDB = context.ProjectTasks.FirstOrDefault(pt => pt.ProjectTaskID == id);
            string fileName = projectTaskDB.TaskData;
            string path = Server.MapPath("~/Content/Uploads/" + fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            //return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}