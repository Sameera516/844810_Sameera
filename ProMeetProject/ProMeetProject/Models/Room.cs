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
    
    public partial class Room
    {
        public int room_id { get; set; }
        public string Vendor_Name { get; set; }
        public string Image { get; set; }
        public bool AC { get; set; }
        public bool Wifi { get; set; }
        public bool Mic { get; set; }
        public bool Podium { get; set; }
        public bool Projector { get; set; }
        public string Location { get; set; }
        public double Price { get; set; }
        public int capacity_of_room { get; set; }
    }
}
