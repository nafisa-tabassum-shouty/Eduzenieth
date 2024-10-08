﻿using EduzeniethFinal.Models;
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
        private EduEntities db = new EduEntities();

        // GET: Student
        public ActionResult Available_Courses()
        {
            if (Session["T_id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int studentId = Convert.ToInt32(Session["T_id"]);

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

            using (var db = new EduEntities())
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
                            Session["ShowAlert_Enrollment"] = true;
                            // Redirect to a success page or any other desired action
                            return RedirectToAction("Enrolled_Courses", "Teacher");
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

            var studentId= Convert.ToInt32(Session["T_id"]);
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

        // GET: Home/Invite_Courses
        [HttpGet]
        public ActionResult Invite_Courses()
        {
            if (Session["T_id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session["ShowAlert"] = null;
            Session["ShowAlert_Enrollment"] = null;

            int teacherId = Convert.ToInt32(Session["T_id"]);
            //  var enrolls = db.Enrolls .Where(c => c.sid== teacherId && c.role==1 && c.status==1).Select(c=>c.id).ToList();
            // var courses = db.Courses.Where(c=>enrolls.Contains(c.Course_Id )).ToList();

            var courses = db.Courses.Where(c => c.teacherID == teacherId).ToList();
            ViewBag.Courses = new SelectList(courses, "Course_Id", "Course_Name");
            ViewBag.Students = null; // Ensure ViewBag.Students is null initially
            ViewBag.SelectedCourseId = null;
            ViewBag.SelectedCourseName = null;

            return View();
        }

        // POST: Home/Invite_Courses
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Invite_Courses(int selectedCourseId)
        {
            int teacherId = Convert.ToInt32(Session["T_id"]);
            var courses = db.Courses.Where(c => c.teacherID == teacherId).ToList();
            ViewBag.Courses = new SelectList(courses, "Course_Id", "Course_Name");
            ViewBag.SelectedCourseId = selectedCourseId;

            // Retrieve course name as a single string
            ViewBag.SelectedCourseName = db.Courses
                                            .Where(c => c.Course_Id == selectedCourseId)
                                            .Select(c => c.Course_Name)
                                            .FirstOrDefault(); // Retrieve the first matching course name

            var sidsForCid = db.Enrolls
                              .Where(e => e.cid == selectedCourseId)
                              .Select(e => e.sid)
                              .ToList();

            var students = db.Students
                             .Where(s => !sidsForCid.Contains(s.StudentID))
                             .ToList();

            ViewBag.Students = students;
            return View();
        }

        // POST: Home/InviteStudents
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InviteStudents(int selectedCourseId, int[] studentIds)
        {
            if (studentIds != null && studentIds.Length > 0)
            {
                foreach (var studentId in studentIds)
                {
                    var enroll = new Enroll
                    {
                        cid = selectedCourseId,
                        sid = studentId,
                        status = 0,
                        role = 3
                    };
                    db.Enrolls.Add(enroll);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Invite_Courses");
        }


        public ActionResult Logout()
        {
            Session["ShowAlert"] = null;
            Session["ShowAlert_Enrollment"] = null;
            Session["T_id"] = null;
            Session["T_username"] = null;
            return RedirectToAction("Index", "Home");
        }
        public JsonResult GetStudents(int courseId)
        {
            var enrolledStudentIds = db.Enrolls.Where(e => e.cid == courseId).Select(e => e.sid).ToList();
            var students = db.Students
                             .Where(s => !enrolledStudentIds.Contains(s.StudentID))
                             .Select(s => new { Value = s.StudentID, Text = s.FirstName + " " + s.LastName })
                             .ToList();

            System.Diagnostics.Debug.WriteLine($"Students for courseId {courseId}: {string.Join(", ", students.Select(s => s.Text))}");

            return Json(students, JsonRequestBehavior.AllowGet);
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
            using (var db = new EduEntities())
            {
                if (username.Equals("admin") && password.Equals("123456"))
                {
                    // Login successful
                    // Redirect to dashboard or any other page
                    Session["admin"] = "logged_in";
                    return RedirectToAction("Login", "Admin");
                }
                else
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
                using (var db = new EduEntities())
                {
                    db.Teachers.Add(teacher);
                    db.SaveChanges();

                    // Set session variables
                    Session["T_id"] = teacher.Id;
                    Session["T_username"] = teacher.Username;

                    Session["ShowAlert"] = true;
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
            Session["ShowAlert"] = null;
            Session["ShowAlert_Enrollment"] = null;
            int teacherId = Convert.ToInt32(Session["T_id"]); // Cast Session["T_id"] to int

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
            Session["ShowAlert"] = null;
            Session["ShowAlert_Enrollment"] = null;
            return View();
        }










        public ActionResult Grade()
        {
            if (Session["T_id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["T_id"] != null)
            {
                int teacherId = Convert.ToInt32(Session["T_id"]);
                var courses = db.Courses.Where(c => c.teacherID == teacherId).ToList();
                ViewBag.Courses = new SelectList(courses, "Course_Id", "Course_Name");
                ViewBag.Students = null; // Ensure ViewBag.Students is null initially
                ViewBag.SelectedCourseId = null;
                ViewBag.SelectedCourseName = null;
            }
            else
            {
                // Handle the case where the session is null
                // Redirect to login or show an error message
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        // POST: Teacher/Grade
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grade(int selectedCourseId)
        {
            int teacherId = Convert.ToInt32(Session["T_id"]);
            var courses = db.Courses.Where(c => c.teacherID == teacherId).ToList();
            ViewBag.Courses = new SelectList(courses, "Course_Id", "Course_Name");
            ViewBag.SelectedCourseId = selectedCourseId;

            // Retrieve course name as a single string
            ViewBag.SelectedCourseName = db.Courses
                                            .Where(c => c.Course_Id == selectedCourseId)
                                            .Select(c => c.Course_Name)
                                            .FirstOrDefault(); // Retrieve the first matching course name

            var sidsForCid = db.Enrolls
                              .Where(e => e.cid == selectedCourseId)
                              .Select(e => e.sid)
                              .ToList();

            var students = db.Students
                             .Where(s => sidsForCid.Contains(s.StudentID))
                             .ToList();

            ViewBag.Students = students;
            return View();
        }


        // POST: Teacher/SubmitGrade
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitGrade(string Grade1, int StudentID, string StudentName, int Course_Id, string Course_Name)
        {
            Session["T_id"] = 1;
            if (Session["T_id"] != null)
            {
                int teacherId = (int)Session["T_id"];
                string teacherName = db.Teachers.Find(teacherId)?.first_name; // Assuming you have a Teachers table

                var grade = new Grade
                {
                    Grade1 = Grade1,
                    GradeDate = DateTime.Now,
                    Course_Id = Course_Id,
                    Course_Name = Course_Name,
                    TeacherID = teacherId,
                    Teacher_Name = teacherName,
                    StudentID = StudentID,
                    StudentName = StudentName
                };

                db.Grades.Add(grade);
                db.SaveChanges();

                return RedirectToAction("Grade");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitGrades(List<GradeViewModel> grades)
        {
            
            if (Session["T_id"] != null)
            {
                int teacherId = Convert.ToInt32(Session["T_id"]);
                string teacherName = db.Teachers.Find(teacherId)?.first_name; // Assuming you have a Teachers table

                foreach (var gradeModel in grades)
                {
                    var grade = new Grade
                    {
                        Grade1 = gradeModel.Grade1,
                        GradeDate = DateTime.Now,
                        Course_Id = gradeModel.Course_Id,
                        Course_Name = gradeModel.Course_Name,
                        TeacherID = teacherId,
                        Teacher_Name = teacherName,
                        StudentID = gradeModel.StudentID,
                        StudentName = gradeModel.StudentName
                    };

                    db.Grades.Add(grade);
                }

                db.SaveChanges();

                return RedirectToAction("Grade");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public class GradeViewModel
        {
            public string Grade1 { get; set; }
            public int StudentID { get; set; }
            public string StudentName { get; set; }
            public int Course_Id { get; set; }
            public string Course_Name { get; set; }


        }

        /*  public ActionResult Cinvitation()
          {
              return View();
          }


          public ActionResult Grade2()
          {
              int teacherId = 1; // This should be dynamically set, e.g., from the logged-in user's session
              var courses = db.Courses.Where(c => c.teacherID == teacherId).ToList();
              ViewBag.Courses = new SelectList(courses, "CourseId", "CourseName");

              var students = db.Students.ToList();
              ViewBag.Students = new SelectList(students, "StudentID", "Email");

              return View();
          }

          // New Action to handle grade submission
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult SubmitStudentGrade(GradeViewModel2 model)
          {
              if (ModelState.IsValid)
              {
                  int teacherId = 1; // This should be dynamically set, e.g., from the logged-in user's session
                  string teacherName = db.Teachers.Find(teacherId)?.first_name; // Assuming you have a Teachers table

                  var grade = new Grade
                  {
                      Grade1 = model.Grade1,
                      GradeDate = DateTime.Now,
                      Course_Id = model.Course_Id,
                      Course_Name = db.Courses.Find(model.Course_Id)?.Course_Name,
                      TeacherID = teacherId,
                      Teacher_Name = teacherName,
                      StudentID = model.StudentID,
                      StudentName = db.Students.Find(model.StudentID)?.FirstName
                  };

                  db.Grades.Add(grade);
                  db.SaveChanges();

                  return RedirectToAction("Grade");
              }

              int teacherIdForDropDowns = 1; // This should be dynamically set, e.g., from the logged-in user's session
              var courses = db.Courses.Where(c => c.teacherID == teacherIdForDropDowns).ToList();
              ViewBag.Courses = new SelectList(courses, "CourseId", "CourseName");

              var students = db.Students.ToList();
              ViewBag.Students = new SelectList(students, "StudentID", "Email");

              return View("Grade", model); // Return the Grade view with the model to display validation errors
          }

          public class GradeViewModel2
          {
              public string Grade1 { get; set; }
              public int StudentID { get; set; }
              public int Course_Id { get; set; }
          }*/








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