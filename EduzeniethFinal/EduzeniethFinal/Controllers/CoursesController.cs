using EduzeniethFinal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace EduzeniethFinal.Controllers
{

    public class CoursesController : Controller
    {
        // GET: Courses/CourseDetails
        public ActionResult CourseDetails()
        {

            int cid = (int)Session["Course_Id"];
            var sidsForCid = new List<int>();
            var tidsForCid = new List<int>();
            var students = new List<Student>();

            var teachers = new List<Teacher>();
            using (var db1 = new EduzenithFinalEntities6())
            {
                // Retrieve sid values for the specific cid
                sidsForCid = db1.Enrolls
                                    .Where(e => e.cid == cid)
                                    .Select(e => e.sid)
                                    .ToList();
                tidsForCid = db1.Courses
                    .Where(e => e.Course_Id == cid && e.teacherID.HasValue)
                    .Select(e => e.teacherID.Value)
                    .ToList();


                // Retrieve student names for each sid
                students = db1.Students
                                 .Where(s => sidsForCid.Contains(s.StudentID))
                                 .ToList();
                teachers = db1.Teachers
                                 .Where(s => tidsForCid.Contains(s.Id))
                                 .ToList();
                var course = db1.Courses.FirstOrDefault(e => e.Course_Id == cid);

                if (course != null)
                {
                    string courseName = course.Course_Name;
                    ViewBag.CourseName = courseName;
                }

                    // Concatenate first names and last names, then store in ViewBag
                    ViewBag.FullNames = students.Select(s => s.FirstName + " " + s.LastName).ToList();
                ViewBag.TFullNames = teachers.Select(s => s.first_name + " " + s.last_name).ToList();

            }


            EduzenithFinalEntities6 db = new EduzenithFinalEntities6();

            // Retrieve enrolled students and teachers from the database
            List<string> enrolledStudents = ViewBag.FullNames;
            List<string> enrolledTeachers = ViewBag.TFullNames;

            // Retrieve previous announcements and associated comments from the database
            List<Announcement> announcements = db.Posts.Where(p => p.PostType == "Announcement" && p.CourseID == cid)
                    .OrderByDescending(p => p.PostDate)
                    .Select(p => new Announcement
                    {
                        PID = p.PostID,
                        Title = p.PostType,
                        Content = p.PostContent,
                        PostedBy = p.UserStatus == 1
                            ? (db.Teachers.FirstOrDefault(t => t.Id == p.UserID) != null
                                ? db.Teachers.FirstOrDefault(t => t.Id == p.UserID).first_name + " " + db.Teachers.FirstOrDefault(t => t.Id == p.UserID).last_name
                                : "")
                            : (db.Students.FirstOrDefault(s => s.StudentID == p.UserID) != null
                                ? db.Students.FirstOrDefault(s => s.StudentID == p.UserID).FirstName + " " + db.Students.FirstOrDefault(s => s.StudentID == p.UserID).LastName
                                : ""),
                        PostedDate = p.PostDate,
                        Comments = db.Comments.Where(c => c.PostID == p.PostID).Select(c => new CommentModel
                        {
                            CommentContent = c.CommentContent,
                            CommentedBy = c.UserStatus == 1
                                ? (db.Teachers.FirstOrDefault(t => t.Id == c.UserID) != null
                                    ? db.Teachers.FirstOrDefault(t => t.Id == c.UserID).first_name + " " + db.Teachers.FirstOrDefault(t => t.Id == c.UserID).last_name
                                    : "")
                                : (db.Students.FirstOrDefault(s => s.StudentID == c.UserID) != null
                                    ? db.Students.FirstOrDefault(s => s.StudentID == c.UserID).FirstName + " " + db.Students.FirstOrDefault(s => s.StudentID == c.UserID).LastName
                                    : "")
                        }).ToList()
                    }).ToList();


            // Pass data to the view
            ViewBag.EnrolledStudents = enrolledStudents;
            ViewBag.EnrolledTeachers = enrolledTeachers;
            ViewBag.Announcements = announcements;

            return View();
        }

        // POST: Classroom/PostAnnouncement
        [HttpPost]
        public ActionResult PostAnnouncement(string announcement)
        {
            EduzenithFinalEntities6  db = new EduzenithFinalEntities6();
            int userId;
            int userStatus;

            if (Session["id"] == null && Session["T_id"] != null)
            {
                userId = (int)Session["T_id"];
                userStatus = 1;
            }
            else
            {
                userId = (int)Session["id"];
                userStatus = 0;
            }

            // Insert the announcement into the database
            Post newPost = new Post
            {
                CourseID = (int)Session["Course_Id"],
                PostContent = announcement,
                PostDate = DateTime.Now,
                UserID = userId,
                UserStatus = userStatus,
                PostType = "Announcement"
            };
            db.Posts.Add(newPost);
            db.SaveChanges();

            // Redirect to the index page after posting the announcement
            return RedirectToAction("CourseDetails", "Courses");
        }

        // POST: Classroom/PostComment
        [HttpPost]
        public ActionResult PostComment(string comment, int postId)
        {
            EduzenithFinalEntities6 db = new EduzenithFinalEntities6();
            int userId;
            int userStatus;

            // Determine user ID and status from session
            if (Session["id"] == null && Session["T_id"] != null)
            {
                userId = (int)Session["T_id"];
                userStatus = 1; // Assuming 1 is for teachers
            }
            else
            {
                userId = (int)Session["id"];
                userStatus = 0; // Assuming 0 is for students
            }

            // Insert the comment into the database for the specified post
            if (postId > 0)
            {
                Comment newComment = new Comment
                {
                    PostID = postId,
                    UserID = userId,
                    UserStatus = userStatus,
                    CommentContent = comment,
                    CommentDate = DateTime.Now
                };
                db.Comments.Add(newComment);
                db.SaveChanges();
            }
            else
            {
                // Handle the case where postId is not assigned a valid value
                // For example, log an error or show an appropriate message to the user
            }

            // Redirect to the course details page after posting the comment
            return RedirectToAction("CourseDetails", "Courses");
        }




























    }
}