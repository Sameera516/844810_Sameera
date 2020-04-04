using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProMeetProject.Models
{
    [MetadataType(typeof(VendorMetaData))]
    public partial class Vendor
    {
        public string Confirm_password { get; set; }
    }
    public class VendorMetaData
    {
        [Required(ErrorMessage = "Vendor id is required")]
        [Display(Name = "Vendor Id")]
        public int Vendor_Id { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Vendor Name is required")]
        [Display(Name = "Vendor name")]
        public string name { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Email id is required")]
        [Display(Name = "Email Id")]
        [DataType(DataType.EmailAddress)]
        public string Email_id { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        public double phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public string address { get; set; }

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