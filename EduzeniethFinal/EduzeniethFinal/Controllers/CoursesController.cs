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
            using (var db1 = new EduzenithFinalEntities4())
            {
                // Retrieve sid values for the specific cid
                var sidsForCid = db1.Enrolls
                                    .Where(e => e.cid == cid)
                                    .Select(e => e.sid)
                                    .ToList();

                // Retrieve student names for each sid
                var students = db1.Students
                                 .Where(s => sidsForCid.Contains(s.StudentID))
                                 .ToList();

                // Store first names and last names in ViewBag
                ViewBag.FirstNames = students.Select(s => s.FirstName).ToList();
                ViewBag.LastNames = students.Select(s => s.LastName).ToList();
            }


            EduzenithFinalEntities4 db = new EduzenithFinalEntities4();

            // Retrieve enrolled students and teachers from the database
            List<string> enrolledStudents = db.Students.Where(s => s.Status == 1).Select(s => s.FirstName + " " + s.LastName).ToList();
            List<string> enrolledTeachers = db.Teachers.Where(t => t.Status == 1).Select(t => t.first_name + " " + t.last_name).ToList();

            // Retrieve previous announcements and associated comments from the database
            List<Announcement> announcements = db.Posts.Where(p => p.PostType == "Announcement")
                    .OrderByDescending(p => p.PostDate)
                    .Select(p => new Announcement
                    {
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
        public ActionResult PostComment(int announcementId, string comment)
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
            // Insert the comment into the database for the specified announcement
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

            // Redirect to the index page after posting the comment
            return RedirectToAction("CourseDetails", "Courses");
        }

        

    }
}