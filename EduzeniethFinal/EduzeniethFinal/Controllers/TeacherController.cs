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
        private EduzenithFinalEntities6 db = new EduzenithFinalEntities6();
        // GET: Teacher
        public ActionResult Login()
        {
            return View();
        }

        // POST: Teacher/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            using (var db = new EduzenithFinalEntities6())
            {
                var teacher = db.Teachers.FirstOrDefault(t => t.Username == username && t.Password == password && t.Status == 1);
                if (teacher != null)
                {
                    // Login successful
                    // Redirect to dashboard or any other page
                    Session["T_id"] = teacher.Id;
                    Session["T_username"] = teacher.Username;
                    return RedirectToAction("Teacher_Details", "Teacher");
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
                using (var db = new EduzenithFinalEntities6())
                {
                    db.Teachers.Add(teacher);
                    db.SaveChanges();

                    // Set session variables
                    Session["T_id"] = teacher.Id;
                    Session["T_username"] = teacher.Username;

                    TempData["ShowAlert"] = true;
                }


                return RedirectToAction("Index", "Home");
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

        public ActionResult Home()
        {
            return View();
        }



       

        public ActionResult Grade()
        {
            ViewBag.Course_Id = new SelectList(db.Courses, "Course_Id", "Course_Name");
            return View();
        }

        // POST: Grades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grade([Bind(Include = "GradeID,Grade1,GradeDate,Course_Id,Course_Name,TeacherID,Teacher_Name,StudentID,StudentName")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                var selectedCourse = db.Courses.Find(grade.Course_Id);
                if (selectedCourse != null)
                {
                    grade.Course_Name = selectedCourse.Course_Name;
                    grade.TeacherID = selectedCourse.teacherID;

                    var teacher = db.Teachers.Find(grade.TeacherID);
                    if (teacher != null)
                    {
                        grade.Teacher_Name = teacher.first_name + " " + teacher.last_name;
                    }
                }

                db.Grades.Add(grade);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Course_Id = new SelectList(db.Courses, "Course_Id", "Course_Name", grade.Course_Id);
            return View(grade);
        }

        // GET: Grades/GetTeacherDetails/5
        public JsonResult GetTeacherDetails(int courseId)
        {
            var course = db.Courses.Find(courseId);
            if (course != null)
            {
                var teacher = db.Teachers.Find(course.teacherID);
                if (teacher != null)
                {
                    return Json(new
                    {
                        teacherID = teacher.Id,
                        teacherName = teacher.first_name + " " + teacher.last_name
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }


    }
}