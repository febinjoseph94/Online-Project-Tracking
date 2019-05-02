using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineProjectTracking.ViewModels
{
    public class EmployeeViewModel
    {
        [Required(ErrorMessage ="Please enter name")]
        [Display(Name ="Employee Name")]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "Please enter designation")]
        [Display(Name = "Designation")]
        public string Designation { get; set; }
        [Display(Name = "Manager Name")]
        public Nullable<int> ManagerID { get; set; }
        [Required(ErrorMessage = "Please enter contact no")]
        [Display(Name = "Contact No")]
        public string ContactNo { get; set; }
        [Required(ErrorMessage = "Please enter email")]
        [Display(Name = "Employee Email")]
        public string EMailID { get; set; }
        [Required(ErrorMessage = "Please enter skills")]
        [Display(Name = "Skill Sets")]
        public string SkillSets { get; set; }
    }
}