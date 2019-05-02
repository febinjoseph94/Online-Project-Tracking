using OnlineProjectTracking.DAL;
using OnlineProjectTracking.Security;
using OnlineProjectTracking.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace OnlineProjectTracking.Controllers
{
    [CustomActionFilter]
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult ViewAllEmployees()
        {
            OPTDBContext context = new OPTDBContext();
            List<Employee> employees = context.Employees.Where(model => model.Designation != "Admin").ToList();
            return View(employees);
        }

        public ActionResult AddNewEmployee()
        {
            ViewBag.Designations = GetDesignationList();
            ViewBag.Managers = GetManagerList();
            return View();
        }

        [HttpPost]
        public ActionResult AddNewEmployee(EmployeeViewModel employee)
        {
            try
            {
                ViewBag.Designations = GetDesignationList();
                ViewBag.Managers = GetManagerList();
                if (!ModelState.IsValid)
                    return View();
                else
                {
                    OPTDBContext dbEntities = new OPTDBContext();
                    if (employee.Designation.Equals("Project Manager"))
                        employee.ManagerID = null;
                    Employee dbemployee = new Employee()
                    {
                        EmployeeName = employee.EmployeeName,
                        Designation = employee.Designation,
                        ContactNo = employee.ContactNo,
                        EMailID = employee.EMailID,
                        ManagerID = employee.ManagerID,
                        SkillSets = employee.SkillSets
                    };
                    dbEntities.Employees.Add(dbemployee);
                    dbEntities.SaveChanges();
                    /// Create account ------
                    UserAccount userAccount = new UserAccount()
                    {
                        EmployeeID = dbemployee.EmployeeID,
                        Email = dbemployee.EMailID,
                        Password = "123",
                    };
                    dbEntities.UserAccounts.Add(userAccount);
                    dbEntities.SaveChanges();
                    TempData["newemployee_success"] = "added";
                    return View();
                }
            }
            catch (System.Exception)
            {
                TempData["newemployee_error"] = "error";
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                OPTDBContext ctx = new OPTDBContext();
                UserAccount userAccount = ctx.UserAccounts.Where(account => account.EmployeeID == id).SingleOrDefault();
                Employee employee = ctx.Employees.Where(emp => emp.EmployeeID == id).SingleOrDefault();
                if (userAccount != null && employee != null)
                {
                    ctx.UserAccounts.Remove(userAccount);
                    ctx.SaveChanges();
                    ctx.Employees.Remove(employee);
                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("ViewAllEmployees");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ViewBag.Designations = GetDesignationList();
            ViewBag.Managers = GetManagerList();
            OPTDBContext dbEntities = new OPTDBContext();
            Employee employee = dbEntities.Employees.Where(model => model.EmployeeID == id).SingleOrDefault();
            return View(employee);
        }

        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            try
            {
                ViewBag.Designations = GetDesignationList();
                ViewBag.Managers = GetManagerList();
                if (!ModelState.IsValid)
                    return View();
                else
                {
                    if (employee.Designation.Equals("Project Manager"))
                        employee.ManagerID = null;
                    using (OPTDBContext dbEntities = new OPTDBContext())
                    {
                        dbEntities.Employees.Add(employee);
                        dbEntities.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                        dbEntities.SaveChanges();
                    }
                    TempData["employee_eidt_success"] = "Employee record updated successfully.";
                    return View(employee);
                }
            }
            catch (System.Exception)
            {
                TempData["employee_eidt_error"] = "Employee record update failed.";
                return View(employee);
            }
        }
        //----------
        private List<SelectListItem> GetDesignationList()
        {
            List<SelectListItem> designation = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Please select", Value = "-1", Disabled=true, Selected= true},
                new SelectListItem {Text = "Developer", Value = "Developer"},
                new SelectListItem {Text = "Designer", Value = "Designer"},
                new SelectListItem {Text = "Project Manager", Value = "Project Manager"},
                new SelectListItem {Text = "Business Analyst", Value = "Business Analyst"},
            };
            return designation;
        }

        private List<SelectListItem> GetManagerList()
        {
            List<SelectListItem> managers = new List<SelectListItem>() { new SelectListItem { Text = "Please select", Value = "-1", Disabled = true, Selected = true } };
            OPTDBContext dbEntities = new OPTDBContext();
            List<Employee> dbmanagers = dbEntities.Employees.Where(employee => employee.Designation.Equals("Project Manager")).ToList();
            foreach (Employee employee in dbmanagers)
            {
                managers.Add(new SelectListItem()
                {
                    Text = employee.EmployeeName,
                    Value = employee.EmployeeID.ToString()
                });
            }
            return managers;
        }
    }
}