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
        private EduzenithFinalEntities6 db = new EduzenithFinalEntities6();

        // GET: Student
        public ActionResult Available_Courses()
        {
            if (Session["id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int studentId = (int)Session["id"];

            // Get the list of course IDs the student is already enrolled in
            var enrolledCourseIds = db.Enrolls
                                      .Where(e => e.sid == studentId && e.status==1)
                                      .Select(e => e.cid)
                                      .ToList();

            // Filter out the courses that the student is already enrolled in
            var availableCourses = db.Courses
                                     .Where(c => !enrolledCourseIds.Contains(c.Course_Id))
                                     .ToList();

            return View(availableCourses);
        }


        [HttpPost]
        public ActionResult Available_Courses(string Course_Code, int Course_Id)
        {
            if (Session["id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new EduzenithFinalEntities6())
            {
                // Find the course by ID
                var course = db.Courses.FirstOrDefault(c => c.Course_Id == Course_Id);

                if (course != null)
                {
                    // Check if the entered Course_Code matches the course's Course_Code
                    if ((Course_Id.ToString() ).Equals( Course_Code))
                    {
                        var student = db.Students.Find(Session["id"]);
                        if (student != null)
                        {
                            // Enroll the student in the course
                            Enroll enrollment = new Enroll
                            {
                                sid = student.StudentID,
                                cid = course.Course_Id,
                                status = 0 // Assuming status 0 indicates pending or initial enrollment
                            };

                            // Add the enrollment record to the database
                            db.Enrolls.Add(enrollment);
                            db.SaveChanges();

                            // Redirect to a success page or any other desired action
                            return RedirectToAction("Enrolled_Courses", "Courses");
                        }
                        else
                        {
                            // Handle the case where the student is not found in the database
                            ViewBag.ErrorMessage = "Student not found.";
                            return View(db.Courses.ToList());
                        }
                    }
                    else
                    {
                        // If the course codes do not match, display an error message
                        ViewBag.ErrorMessage = "Invalid Course Code.";
                        return View(db.Courses.ToList());
                    }
                }
                else
                {
                    // If the course is not found, display an error message
                    ViewBag.ErrorMessage = "Course not found.";
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