using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProMeetProject.Models
{
    [MetadataType(typeof(BookingMetaData))]
    public partial class Booking
    {
        public double Card_Number { get; set; }
        public int expiry_month { get; set; }
        public int expiry_year { get; set; }
        public int cvv { get; set; }
        public string Name_on_the_card { get; set; }

    }
    public class BookingMetaData
    {
        [Required(ErrorMessage ="Employee id is required")]
        [Display(Name ="Employee Id")]
        public int Emp_Id { get; set; }


        [Required(ErrorMessage = "Employee name is required")]
        [Display(Name = "Employee name")]
        public string Emp_Name { get; set; }


        [Required(ErrorMessage = "Employee email is required")]
        [Display(Name = "Employee email_id")]
        public string Emp_Email_Id { get; set; }


        [Required(ErrorMessage = "Room id is required")]
        [Display(Name = "Room id")]
        public int room_id { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "Location")]
        public string location { get; set; }



        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "DateTime")]
        [DataType(DataType.Date)]
        public System.DateTime DateTime { get; set; }


        [Required(ErrorMessage = "Duration is required")]
        [Display(Name = "Duration")]
        public int Duration { get; set; }


        [Required(ErrorMessage = "Payment money is required")]
        [Display(Name = "Payment money")]
        public decimal Payment_money { get; set; }

        [Required(ErrorMessage ="Card Number is required")]
        [Display(Name ="Card Number")]
        public double Card_Number { get; set; }

        [Required(ErrorMessage ="Expiry month is required")]
        [Display(Name ="Expiry Month")]
        public int expiry_month{ get; set; }


        [Required(ErrorMessage ="Expiry year is required")]
        [Display(Name ="Expiry year")]
        public int expiry_year { get; set; }

        [Required(ErrorMessage ="Cvv is required")]
        [Display(Name ="CVV")]
        public int cvv { get; set; }
        
        [Required(ErrorMessage ="Name on the card is required")]
        [Display(Name ="Name on the card")]
        public string Name_on_the_card { get; set; }


    }
}