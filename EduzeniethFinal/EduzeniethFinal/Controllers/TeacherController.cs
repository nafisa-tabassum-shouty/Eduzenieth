using EduzeniethFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace EduzeniethFinal.Controllers
{
    public class TeacherController : Controller
    {
        private EduzenithFinalEntities7 db = new EduzenithFinalEntities7();

        // GET: Student
        public ActionResult Available_Courses()
        {
            if (Session["T_id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int studentId = (int)Session["T_id"];

            // Get the list of course IDs the student is already enrolled in
            var enrolledCourseIds = db.Enrolls
                                      .Where(e => e.sid == studentId && e.status == 1 && e.role==1)
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
            if (Session["T_id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new EduzenithFinalEntities7())
            {
                // Find the course by ID
                var course = db.Courses.FirstOrDefault(c => c.Course_Id == Course_Id);

                if (course != null)
                {
                    // Check if the entered Course_Code matches the course's Course_Code
                    if ((Course_Id.ToString()).Equals(Course_Code))
                    {
                        var student = db.Teachers.Find(Session["T_id"]);
                        if (student != null)
                        {
                            // Enroll the student in the course
                            Enroll enrollment = new Enroll
                            {
                                sid = student.Id,
                                cid = course.Course_Id,
                                status = 0, // Assuming status 0 indicates pending or initial enrollment
                                role = 1
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
                            ViewBag.ErrorMessage = "Teacher not found.";
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



        public ActionResult Enrolled_Courses()
        {
            if (Session["T_id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var studentId = (int)Session["T_id"];
            var student = db.Teachers.Find(studentId);
            if (student == null)
            {
                return HttpNotFound();
            }

            // Retrieve enrolled courses for the student
            var enrollments = db.Enrolls.Where(e => e.sid == studentId && e.status == 1 && e.role==1).ToList();


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
            Session["T_id"] = null;
            Session["T_username"] = null;
            return RedirectToAction("Index", "Home");
        }


        // GET: Teacher
        public ActionResult Login()
        {
            return View();
        }

        // POST: Teacher/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            using (var db = new EduzenithFinalEntities7())
            {
                var teacher = db.Teachers.FirstOrDefault(t => t.Username == username && t.Password == password && t.Status == 1);
                if (teacher != null)
                {
                    // Login successful
                    // Redirect to dashboard or any other page
                    Session["T_id"] = teacher.Id;
                    Session["T_username"] = teacher.Username;
                    return RedirectToAction("Home", "Teacher");
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
                using (var db = new EduzenithFinalEntities7())
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
            if (Session["T_id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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