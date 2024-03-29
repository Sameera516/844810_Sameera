﻿using ProMeetProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProMeetProject.Controllers
{
    public class VendorController : Controller
    {
        public ActionResult Registration()
        {
            return View();
        }
        //Registration post action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")]Vendor v)
        {
            bool Status = false;
            string Message = "";

            //Model is valid or not
            if (ModelState.IsValid)
            {
                #region//email already exists or not
                var isexists = IsEmailExists(v.Email_id);
                if (isexists)
                {
                    ModelState.AddModelError("Email exists", "Email already exists");
                    return View(v);
                }
                #endregion
                #region Generate Activation Code
                v.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password hashing
                v.password = Crypto.Hash(v.password);
                v.Confirm_password = Crypto.Hash(v.Confirm_password);
                #endregion
                v.IsEmailVerified = false;

                #region Save to database
                using (FinalDatabaseEntities1 fp = new FinalDatabaseEntities1())
                {
                    fp.Vendors.Add(v);
                    fp.SaveChanges();
                    SendVerificationLinkEmail(v.Email_id, v.ActivationCode.ToString());
                    Message = "Registration successfully done.Account activation link" + " has been sent to your email_id" + v.Email_id;
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
            return View(v);
        }


        //Verify Account  

        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (FinalDatabaseEntities1 dc = new FinalDatabaseEntities1())
            {
                dc.Configuration.ValidateOnSaveEnabled = false;
                var v = dc.Vendors.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
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
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(VendorLogin login, string ReturnUrl)
        {
            string message = "";
            using (FinalDatabaseEntities1 dc = new FinalDatabaseEntities1())
            {
                var c = dc.Vendors.Where(a => a.Email_id == login.email_id).FirstOrDefault();
                if (c != null)
                {
                    if (string.Compare(Crypto.Hash(login.password), c.password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20;
                        var ticket = new FormsAuthenticationTicket(login.email_id, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                        return RedirectToAction("Index");
                        /*if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }*/
                    }
                    else
                    {
                        //message = "Invalid credential provided";
                        ModelState.AddModelError("", "The username or password is incorrect");

                    }
                }
                else
                {
                    //message = "Invalid credential provided";
                    ModelState.AddModelError("", "The username or password is incorrect");
                }
            }
            //ViewBag.Message = message;
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [NonAction]
        public bool IsEmailExists(string email_id)
        {
            using (FinalDatabaseEntities1 fp = new FinalDatabaseEntities1())
            {
                var c = fp.Vendors.Where(a => a.Email_id == email_id).FirstOrDefault();
                return c != null;
            }

        }
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Vendor/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            MailAddress fromEmail = new MailAddress("keerthisudhamathangi@gmail.com");
            MailAddress toEmail = new MailAddress(emailID);
            var fromEmailPassword = "keerthi34"; // Replace with actual password
            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";

                body = "<br/><br/>We are excited to tell you that your account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/><br/>We got request for reset your account password.Please Click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">" + link + "</a>";


            }
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

        //Forgot password
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string EmailId)
        {
            string message = "";
            bool status = false;

            using (FinalDatabaseEntities1 dc = new FinalDatabaseEntities1())
            {
                var acc = dc.Vendors.Where(a => a.Email_id == EmailId).FirstOrDefault();
                if (acc != null)
                {
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(acc.Email_id, resetCode, "ResetPassword");
                    message = "Password Reset Link Successfully Sent to Your Mail Id";

                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();

                }
                else
                {
                    ModelState.AddModelError("EmailExist", "Account Not Found with Provided mail Id Please Check the Mail Id and Try again");

                }
                ViewBag.Message = message;
            }
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            using (FinalDatabaseEntities1 dc = new FinalDatabaseEntities1())
            {
                var user = dc.Vendors.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (FinalDatabaseEntities1 dc = new FinalDatabaseEntities1())
                {
                    var user = dc.Vendors.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.password = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "New Password has been successfully updated";
                    }
                }
            }
            else
            {
                message = "Something went wrong";
            }
            ViewBag.Message = message;
            return View(model);
        }
    }
}