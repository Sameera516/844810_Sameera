using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using ProMeetProject.Models;
using System.IO;

namespace ProMeetProject.Controllers
{
    public class RoomController : Controller
    {
        // GET: Room
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Room room)
        {
            bool Status = false;
            string message = "";
            var isexists = IsNameExists(room.Vendor_Name);
            if (!isexists)
            {
                ModelState.AddModelError("Not exists", "Vendor Name does not exists");
                return View(room);
            }
            string fileName = Path.GetFileNameWithoutExtension(room.ImageFile.FileName);
            string extension = Path.GetExtension(room.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            room.Image = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            room.ImageFile.SaveAs(fileName);
            using (FinalDatabaseEntities1 db = new FinalDatabaseEntities1())
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                message = "Room details added successfully!";
                Status = true;
            }
            ViewBag.Status = Status;
            ViewBag.Message = message;
            return View(room);
        }
        [HttpGet]
        public ActionResult Display()
        {
            FinalDatabaseEntities1 db = new FinalDatabaseEntities1();
            return View(db.Rooms.ToList());
        }

        [NonAction]
        public bool IsNameExists(string name)
        {
            using (FinalDatabaseEntities1 fp = new FinalDatabaseEntities1())
            {
                var c = fp.Vendors.Where(a => a.name == name).FirstOrDefault();
                return c != null;
            }

        }


    }
}