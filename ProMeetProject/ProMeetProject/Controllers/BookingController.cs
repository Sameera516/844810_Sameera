using ProMeetProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ProMeetProject.Controllers
{
    public class BookingController : Controller
    {
        public ActionResult Registration()
        {
            return View();
        }
        //Registration post action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "Is_PaymentDone,ActivationCode")] Booking booking)
        {
            bool Status = false;
            string Message = "";

            //Model is valid or not
            if (ModelState.IsValid)
            {
                //Emp_Id exists or not
                var Emp_id = IsEmp_IdExists(booking.Emp_Id);
                if (!Emp_id)
                {
                    ModelState.AddModelError("EmpId Not exists", "Employee Id does not exists");
                    return View(booking);
                }
                var Emp_Name = IsEmp_NameExists(booking.Emp_Name);
                if (!Emp_Name)
                {
                    ModelState.AddModelError("EmpName Not exists", "Employee Name does not exists");
                    return View(booking);
                }
                var id_exists = IsRoomIdExists(booking.room_id);
                if (!id_exists)
                {
                    ModelState.AddModelError("room id Not exists", "Room ID does not exists");
                    return View(booking);
                }
                var Emp_Email_ID = IsEmp_EmailExists(booking.Emp_Email_Id);
                if (!Emp_Email_ID)
                {
                    ModelState.AddModelError("Emp_Email Not exists", "Employee Mail does not exists");
                    return View(booking);
                }


                booking.Is_PaymentDone = false;

                #region Save to database
                using (FinalDatabaseEntities1 fp = new FinalDatabaseEntities1())
                {
                    booking.Is_PaymentDone = true;
                    fp.Bookings.Add(booking);
                    fp.SaveChanges();
                    SendVerificationLinkEmail(booking.Emp_Email_Id,booking.ActivationCode.ToString());
                    Message="Your Booking request has been sent to the admin.You will receive a mail whether booking is confirmed or not from the admin in a while";
                    Status = true;
                }
                #endregion
            }
            else
            {
                Message = "Invalid request";
            }
            ViewBag.Message = Message;
            ViewBag.Status = Status;
            return View(booking);
        }
       
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (FinalDatabaseEntities1 dc = new FinalDatabaseEntities1())
            {
                dc.Configuration.ValidateOnSaveEnabled = false;
                var v = dc.Bookings.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.Is_PaymentDone = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;

            return View();
        }
        [NonAction]
        public bool IsRoomIdExists(int id)
        {
            using (FinalDatabaseEntities1 fp = new FinalDatabaseEntities1())
            {
                var c = fp.Rooms.Where(a => a.room_id == id).FirstOrDefault();
                return c != null;
            }

        }
        [NonAction]
        public bool IsEmp_IdExists(int id)
        {
            using (FinalDatabaseEntities1 fp = new FinalDatabaseEntities1())
            {
                var c = fp.Employees.Where(a => a.Emp_Id == id).FirstOrDefault();
                return c != null;
            }

        }
        [NonAction]
        public bool IsEmp_NameExists(string name)
        {
            using (FinalDatabaseEntities1 fp = new FinalDatabaseEntities1())
            {
                var c = fp.Employees.Where(a => a.Name == name).FirstOrDefault();
                return c != null;
            }

        }
        [NonAction]
        public bool IsEmp_EmailExists(string email)
        {
            using (FinalDatabaseEntities1 fp = new FinalDatabaseEntities1())
            {
                var c = fp.Employees.Where(a => a.email_id == email).FirstOrDefault();
                return c != null;
            }

        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID,string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Booking/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            MailAddress fromEmail = new MailAddress("keerthisudhamathangi@gmail.com");
            MailAddress toEmail = new MailAddress(emailID);
            var fromEmailPassword = "keerthi34"; // Replace with actual password
            string subject = "Meeting Room Booking Notification";
            string body = "<br/><br/>Your Meeting Room details and your payment has been received." +
                "<br/><br/>Thank you for associating with us.Your Booking request has been sent to the admin.You will receive a mail whether booking is confirmed or not from the admin in a while" ;

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                try
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword);
                    smtp.Send(message);

                }
                catch (Exception)
                {

                }

        }

        public ActionResult Display()//Delete booked rooms for admin
        {
            FinalDatabaseEntities1 db = new FinalDatabaseEntities1();
            return View(db.Bookings.ToList());
        }
        public ActionResult Edit(int? id)
        {
            FinalDatabaseEntities1 db = new FinalDatabaseEntities1();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Booking_Id,Emp_Id,Emp_Name,Emp_Email_Id,room_id,location,DateTime,Duration,Payment_money,Is_PaymentDone,ActivationCode")] Booking booking)
        {
            
            string message = "";
            FinalDatabaseEntities1 db = new FinalDatabaseEntities1();
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                message = "Booking has been confirmed";
            }
            else
            {
               ModelState.AddModelError(" ","Booking details cannot be edited");
            }
            ViewBag.Message = message;
            return View(booking);
        }

        public ActionResult ConfirmBooking(string email)
        {
            string message = "";
            Booking booking = new Booking();
            using(FinalDatabaseEntities1 db=new FinalDatabaseEntities1())
            {
                booking.IsBookingConfirmed = true;
                db.SaveChanges();
                SendBookingConfirmedEmail(booking.Emp_Email_Id);
                message = "The meeting room booking is confirmed";
            }
            ViewBag.Message = message;
            return View(booking);
        }
        public void SendBookingConfirmedEmail(string emailID)
        {
          MailAddress fromEmail = new MailAddress("keerthisudhamathangi@gmail.com");
            MailAddress toEmail = new MailAddress(emailID);
            var fromEmailPassword = "keerthi34"; // Replace with actual password
            string subject = "Meeting Room Booking Notification";
            string body = "<br/><br/>Hi,Your Meeting Room has been successfully booked.Thank you for associating with us";
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                try
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword);
                    smtp.Send(message);

                }
                catch (Exception)
                {

                }

        }

        public ActionResult Index()//To display booked rooms
        {
            FinalDatabaseEntities1 db = new FinalDatabaseEntities1();
            return View(db.Bookings.ToList());
        }

        public ActionResult Delete(int? id)//Delete booked room
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinalDatabaseEntities1 db = new FinalDatabaseEntities1();
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)//Delete booked room
        {
            string message = "";
            FinalDatabaseEntities1 db = new FinalDatabaseEntities1();
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            message = "Your booking has been cancelled";
            ViewBag.Message = message;
            return View(booking);
        }
    }
}