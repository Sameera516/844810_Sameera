using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ProMeetProject.Models
{
    [MetadataType(typeof(RoomMetaData))]
    public partial class Room
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }
    public partial class RoomMetaData
    {
        [Required(ErrorMessage = "Room id is required")]
        [Display(Name = "Room Id")]
        public int room_id { get; set; }


        [Required(ErrorMessage = "Vendor name is required")]
        [Display(Name = "Vendor Name")]
        public string Vendor_Name { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please upload the image")]
        [Display(Name = "Upload Image")]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [Required]
        public bool AC { get; set; }


        [Required]
        public bool Wifi { get; set; }


        [Required]
        public bool Mic { get; set; }


        [Required]
        public bool Podium { get; set; }


        [Required]
        public bool Projector { get; set; }




        [Required(AllowEmptyStrings = false, ErrorMessage = "Location is required")]
        public string Location { get; set; }


        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }


        [Required(ErrorMessage = "Capacity is required")]
        public int capacity_of_room { get; set; }


        public HttpPostedFileBase ImageFile { get; set; }

    }
}