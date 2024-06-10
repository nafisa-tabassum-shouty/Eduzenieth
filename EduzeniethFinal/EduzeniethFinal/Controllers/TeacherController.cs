using EduzeniethFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EduzeniethFinal.Controllers
{
    public class TeacherController : Controller
    {
        private EduzenithFinalEntities4 db = new EduzenithFinalEntities4();
        // GET: Teacher
        public ActionResult Login()
        {
            return View();
        }

        // POST: Teacher/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            using (var db = new EduzenithFinalEntities4())
            {
                var teacher = db.Teachers.FirstOrDefault(t => t.Username == username && t.Password == password && t.Status == 1);
                if (teacher != null)
                {
                    // Login successful
                    // Redirect to dashboard or any other page
                    Session["T_id"] = teacher.Id;
                    Session["T_username"] = teacher.Username;
                    return RedirectToAction("Available_Courses", "Teacher");
                }
                else
                {
                    ViewBag.Log = "Login Failed. Please enter correct Username and Password";
                    // Login failed
                    // You can add a ViewBag message here to show an error message on the login page
                    ViewBag.ErrorMessage = "Invalid username or password";
                    return View();
                }
            }
        }











        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "Id,Username,Password,first_name,last_name,father_name,Email,Designation,MaritalStatus,MobileNumber,Nationality,Religion,Address,Gender,EducationalField,NID_PassportNo")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                // Assuming "db" is your DbContext instance
                using (var db = new EduzenithFinalEntities4())
                {
                    db.Teachers.Add(teacher);
                    db.SaveChanges();

                    // Set session variables
                    Session["T_id"] = teacher.Id;
                    Session["T_username"] = teacher.Username;
                }

                return RedirectToAction("Available_Courses", "Student");
            }

            return View(teacher);
        }
      

















        public ActionResult Teacher_Details()
        {
            if (Session["T_id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int teacherId = (int)Session["T_id"]; // Cast Session["T_id"] to int

            var teacher = db.Teachers.Find(teacherId);
            if (teacher == null)
            {
                return HttpNotFound();
            }

            return View(teacher);
        }


    }
}