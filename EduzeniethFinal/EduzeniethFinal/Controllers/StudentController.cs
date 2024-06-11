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
    public class StudentController : Controller
    {
        private EduzenithFinalEntities4 db = new EduzenithFinalEntities4();
        // GET: Student
        public ActionResult Available_Courses()
        {
            if (Session["id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(db.Courses.ToList());
        }
        [HttpPost]
        public ActionResult Available_Courses(string Course_Id)
        {
            if (Session["id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = new EduzenithFinalEntities4())
            {
                // Find the course by Course_Id
                string courseIdString = Course_Id;
                int courseIdInt = Convert.ToInt32(courseIdString);

                var course = db.Courses.ToList().FirstOrDefault(c => c.Course_Id == courseIdInt);
                

                if (course != null)
                {
                    var student = db.Students.Find(Session["id"]);
                    // Assuming you have the student ID available, replace 'studentId' with the actual student ID
                    int studentId = student.StudentID; // place this with the actual student ID

                    // Enroll the student in the course
                    Enroll enrollment = new Enroll
                    {
                        sid = studentId,
                        cid = course.Course_Id,
                        status = 0 // You can set the status to indicate enrollment status (e.g., 1 for enrolled)
                    };

                    // Add the enrollment record to the database
                    db.Enrolls.Add(enrollment);
                    db.SaveChanges();

                    // Redirect to a success page or any other desired action
                    return RedirectToAction("EnrollmentSuccess");
                }
                else
                {
                    
                    ViewBag.Log = "Invalid Course Code";
                    ViewBag.ErrorMessage = "Invalid Course Code";
                    return View(db.Courses.ToList());
                }
            }
        }

        public ActionResult Student_Details()
        {
            if (Session["id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = db.Students.Find(Session["id"]);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }
        public ActionResult Enrolled_Courses()
        {
            if (Session["id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var studentId = (int)Session["id"];
            var student = db.Students.Find(studentId);
            if (student == null)
            {
                return HttpNotFound();
            }

            // Retrieve enrolled courses for the student
            var enrollments = db.Enrolls.Where(e => e.sid == studentId && e.status == 1).ToList();
            

            // Retrieve course details for each enrollment
            List<Course> courses = new List<Course>();
            foreach (var enrollment in enrollments)
            {
                Console.WriteLine(enrollment.cid);
                var course = db.Courses.FirstOrDefault(c => c.Course_Id == enrollment.cid);
                if (course != null)
                {
                    courses.Add(course);
                }
            }

            return View(courses);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enrolled_Courses(string Course_Id)
        {
            
                Session["Course_Id"] = Convert.ToInt32(Course_Id);
                return RedirectToAction("CourseDetails", "Courses");
            
           
            return View();
        }
        public ActionResult Logout()
        {
            Session["id"] = null;
            Session["username"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Home()
        {
            return View();
        }


    }
}