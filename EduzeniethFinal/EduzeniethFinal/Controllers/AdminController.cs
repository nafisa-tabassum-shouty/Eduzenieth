using EduzeniethFinal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduzeniethFinal.Controllers
{

    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AddCourse()
        {
            return View();
        }

        private EduzenithFinalEntities4 db = new EduzenithFinalEntities4();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCourse([Bind(Include = "Course_Code,Course_Name,teacherID,Course_desc")] Course course)
        {
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
            return View();
        }

        // GET: Admin/Manage_Students
        public ActionResult Manage_Students()
        {
            var students = db.Students.ToList();
            return View(students);
        }

        // GET: Admin/Create
        public ActionResult Add_Student()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Student(Student student)
        {
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
            var student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Manage_Students");
        }
















        public ActionResult Student_PendingRegistration()
        {
            var pendingStudents = db.Students.Where(s => s.Status == 0).ToList();
            return View(pendingStudents);
        }

        [HttpPost]
        public ActionResult Student_PendingRegistration_Accept(int id)
        {
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
            var pendingTeachers = db.Teachers.Where(t => t.Status == 0).ToList();
            return View(pendingTeachers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TeacherPendingRegistration_Accept(int id)
        {
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
            var pendingStudents = db.Enrolls.Where(s => s.status == 0).ToList();
            return View(pendingStudents);
        }
        [HttpPost]
        public ActionResult Student_Pending_Enrollment_Accept(int id)
        {
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
            var student = db.Enrolls.Find(id);
            if (student != null)
            {
                db.Enrolls.Remove(student);
                db.SaveChanges();
            }
            return RedirectToAction("Student_Pending_Enrollment");
        }




    }
}