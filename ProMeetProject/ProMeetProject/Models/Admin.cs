using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProMeetProject.Models;
using System.ComponentModel.DataAnnotations;

namespace ProMeetProject.Models
{
    public class Admin
    {
        [Key]
        [Required(ErrorMessage = "Username is required")]
        public string username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }

    }
}