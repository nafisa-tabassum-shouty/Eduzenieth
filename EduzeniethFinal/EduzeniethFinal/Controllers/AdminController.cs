using EduzeniethFinal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EduzeniethFinal.Controllers
{

    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AddCourse()
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        private EduzenithFinalEntities4 db = new EduzenithFinalEntities4();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCourse([Bind(Include = "Course_Code,Course_Name,teacherID,Course_desc")] Course course)
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                course.Created_at = DateTime.Now; // Set Created_at here
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        public ActionResult Home()
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        // GET: Admin/Manage_Students
        public ActionResult Manage_Students()
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var students = db.Students.ToList();
            return View(students);
        }

        // GET: Admin/Create
        public ActionResult Add_Student()
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Student(Student student)
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Manage_Students");
            }
            return View(student);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student)
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage_Students");
            }
            return View(student);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete_Student(int id)
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Manage_Students");
        }
















        public ActionResult Student_PendingRegistration()
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pendingStudents = db.Students.Where(s => s.Status == 0).ToList();
            return View(pendingStudents);
        }

        [HttpPost]
        public ActionResult Student_PendingRegistration_Accept(int id)
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = db.Students.Find(id);
            if (student != null)
            {
                student.Status = 1;
                db.SaveChanges();
            }
            return RedirectToAction("Student_PendingRegistration");
        }

        [HttpPost]
        public ActionResult Student_PendingRegistration_Decline(int id)
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = db.Students.Find(id);
            if (student != null)
            {
                db.Students.Remove(student);
                db.SaveChanges();
            }
            return RedirectToAction("Student_PendingRegistration");
        }








        public ActionResult TeacherPendingRegistration()
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pendingTeachers = db.Teachers.Where(t => t.Status == 0).ToList();
            return View(pendingTeachers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TeacherPendingRegistration_Accept(int id)
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var teacher = db.Teachers.Find(id);
            if (teacher != null)
            {
                teacher.Status = 1;
                db.SaveChanges();
            }
            return RedirectToAction("TeacherPendingRegistration");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TeacherPendingRegistration_Decline(int id)
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var teacher = db.Teachers.Find(id);
            if (teacher != null)
            {
                db.Teachers.Remove(teacher);
                db.SaveChanges();
            }
            return RedirectToAction("TeacherPendingRegistration");
        }



















        public ActionResult Student_Pending_Enrollment()
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pendingStudents = db.Enrolls.Where(s => s.status == 0).ToList();
            return View(pendingStudents);
        }
        [HttpPost]
        public ActionResult Student_Pending_Enrollment_Accept(int id)
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = db.Enrolls.Find(id);
            if (student != null)
            {
                student.status = 1;
                db.SaveChanges();
            }
            return RedirectToAction("Student_Pending_Enrollment");
        }

        [HttpPost]
        public ActionResult Student_Pending_Enrollment_Decline(int id)
        {
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = db.Enrolls.Find(id);
            if (student != null)
            {
                db.Enrolls.Remove(student);
                db.SaveChanges();
            }
            return RedirectToAction("Student_Pending_Enrollment");
        }


















        public ActionResult Login()
        {
            return View();
        }

        // POST: Student/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {

            if (username.Equals("admin") && password.Equals("123456"))
            {
                // Login successful
                // Redirect to dashboard or any other page
                Session["admin"] = "logged_in";
                return RedirectToAction("Home", "Admin");
            }
            else
            {
                ViewBag.Log = "Login Failed.Please Enter Correct Username and Password";
                // Login failed
                // You can add a ViewBag message here to show an error message on the login page
                ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }

        }
        public ActionResult Logout()
        {
            Session["admin"] = null;
            return RedirectToAction("Index", "Home");
        }





































        public ActionResult Manage_Courses()
        {
            return View(db.Courses.ToList());
        }
        // GET: Courses/Delete/5
        public ActionResult Delete_Course(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete_Course")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed_Course(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Delete_Course","Courses");
        }



    }
}