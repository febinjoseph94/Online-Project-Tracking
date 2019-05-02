using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineProjectTracking.ViewModels
{
    public class ProjectViewModel
    {
        [Display(Name = "Project Name")]
        [Required(ErrorMessage = "name required")]
        public string ProjectName { get; set; }

        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "required")]
        public Nullable<System.DateTime> StartDate { get; set; }

        [Display(Name ="End Date")]
        [Required(ErrorMessage ="required")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Display(Name ="Client Name")]
        [Required(ErrorMessage ="required")]
        public string ClientName { get; set; }

        [Display(Name ="Description")]
        [Required(ErrorMessage ="required")]
        public string ProjectDescription { get; set; }
    }
}