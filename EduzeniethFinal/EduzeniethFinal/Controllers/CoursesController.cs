using EduzeniethFinal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace EduzeniethFinal.Controllers
{
    
    public class CoursesController : Controller
    {
        // GET: Courses/CourseDetails
        public ActionResult CourseDetails()
        {

            int cid = (int)Session["Course_Id"];
            var sidsForCid= new List<int>();
            var tidsForCid = new List<int>();
            var students = new List<Student>();

            var teachers = new List<Teacher>();
            using (var db1 = new EduzenithFinalEntities4())
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

                // Concatenate first names and last names, then store in ViewBag
                ViewBag.FullNames = students.Select(s => s.FirstName + " " + s.LastName).ToList();
                ViewBag.TFullNames = teachers.Select(s => s.first_name + " " + s.last_name).ToList();

            }


            EduzenithFinalEntities4 db = new EduzenithFinalEntities4();

            // Retrieve enrolled students and teachers from the database
            List<string> enrolledStudents = ViewBag.FullNames;
            List<string> enrolledTeachers = ViewBag.TFullNames;

            // Retrieve previous announcements and associated comments from the database
            List<Announcement> announcements = db.Posts.Where(p => p.PostType == "Announcement" && p.CourseID==cid)
                    .OrderByDescending(p => p.PostDate)
                    .Select(p => new Announcement
                    {
                        PID=p.PostID,
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
                        Comments = p.Comments.Select(c => c.CommentContent).ToList()
                    })
                    .ToList();


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
            EduzenithFinalEntities4 db = new EduzenithFinalEntities4();
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
            return RedirectToAction("CourseDetails","Courses");
        }

        // POST: Classroom/PostComment
        [HttpPost]
        public ActionResult PostComment( string comment)
        {
            EduzenithFinalEntities4 db = new EduzenithFinalEntities4();
            int userId;
            int userStatus;
            int announcementId=-1;
            if (ViewBag.postId != null && ViewBag.postId is int)
            {
                announcementId = (int)ViewBag.postId;
            }
            else
            {
                // Handle the case where the value is null or not of type int
                // For example, you could assign a default value or show an error message
                // announcementId = defaultValue;
            }
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
            // Insert the comment into the database for the specified announcement
            if (announcementId != 0) // Replace 0 with the default value if different
            {
                Comment newComment = new Comment
                {
                    PostID = announcementId,
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
                // Handle the case where announcementId is not assigned a valid value
                // For example, log an error or show an appropriate message to the user
            }


            // Redirect to the index page after posting the comment
            return RedirectToAction("CourseDetails", "Courses");
        }

        

    }
}