//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProMeetProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Booking
    {
        public int Booking_Id { get; set; }
        public int Emp_Id { get; set; }
        public string Emp_Name { get; set; }
        public string Emp_Email_Id { get; set; }
        public int room_id { get; set; }
        public string location { get; set; }
        public System.DateTime DateTime { get; set; }
        public int Duration { get; set; }
        public decimal Payment_money { get; set; }
        public bool Is_PaymentDone { get; set; }
        public System.Guid ActivationCode { get; set; }
        public bool IsBookingConfirmed { get; set; }
    }
}
