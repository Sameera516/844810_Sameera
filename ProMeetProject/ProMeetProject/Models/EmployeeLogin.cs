using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProMeetProject.Models
{
    public class EmployeeLogin
    {
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email id is required")]
        [Display(Name = "Email id")]
        public string email_id { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }


        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

    }
}