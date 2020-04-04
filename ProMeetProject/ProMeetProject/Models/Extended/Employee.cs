using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProMeetProject.Models
{
    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {
        public string Confirm_password { get; set; }
    }
    public class EmployeeMetadata
    {
        [Required(ErrorMessage = "Employee id is required")]
        [Display(Name = "Employee id")]
        public int Emp_Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Employee Name is required")]
        [Display(Name = "Employee name")]
        public string Name { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Email id is required")]
        [Display(Name = "Email Id")]
        [DataType(DataType.EmailAddress)]
        public string email_id { get; set; }


        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        public double phone_number { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Address is required")]
        public string Address { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "password should contain atleast 6 characters")]
        public string password { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "password and confirm password should match")]
        [Display(Name = "Confirm password")]
        public string Confirm_password { get; set; }




    }
}