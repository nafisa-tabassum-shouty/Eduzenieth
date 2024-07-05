using EduzeniethFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduzeniethFinal.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        private EduzenithFinalEntities7 db = new EduzenithFinalEntities7();

        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "StudentID,Password,Nationality,MobileNumber,FirstName,Email,LastName,FatherName,Religion,Address,DateOfBirth,BloodGroup,Username")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                Session["id"] = student.StudentID;
                Session["username"] = student.Username;
                TempData["ShowAlert"] = true;
                return RedirectToAction("Index", "Home");
            }

            return View(student);
        }
        public ActionResult Login()
        {
            return View();
        }

        // POST: Student/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            using (var db = new EduzenithFinalEntities7 ()) // Replace YourDbContext with your actual DbContext
            {
                var student = db.Students.FirstOrDefault(s => s.Username == username && s.Password == password && s.Status == 1);
                if (student != null)
                {
                    // Login successful
                    // Redirect to dashboard or any other page
                    Session["id"] = student.StudentID;
                    Session["username"] = student.Username;
                    return RedirectToAction("Enrolled_Courses", "Student");
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
        }













        private static List<string> imagePaths = new List<string>
    {
        "~/Image/Slider_1.png",
        "~/Image/Slider_2.png",
        "~/Image/Slider_3.png"
    };

        private static int currentIndex = 0;

        [HttpGet]
        public JsonResult GetNextImage()
        {
            currentIndex = (currentIndex + 1) % imagePaths.Count;
            return Json(new { imagePath = Url.Content(imagePaths[currentIndex]) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPreviousImage()
        {
            currentIndex = (currentIndex - 1 + imagePaths.Count) % imagePaths.Count;
            return Json(new { imagePath = Url.Content(imagePaths[currentIndex]) }, JsonRequestBehavior.AllowGet);
        }




    }

}