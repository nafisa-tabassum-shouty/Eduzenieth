using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EduzeniethFinal.Models;

namespace EduzeniethFinal.Controllers
{
    public class QuestionsController : Controller
    {
        private EduEntities db = new EduEntities();

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            // Retrieve the list of questions
            var questions = db.Questions.ToList();

            // Retrieve student information from the session
            int studentID = (int)Session["id"];
            string studentName = Session["username"].ToString();
            int totalMarks = 0;
            int fullMarks = questions.Count * 2;
            // A list to store the result with user's answers
            var resultList = new List<ResultViewModel>();

            foreach (var question in questions)
            {
                // Get the selected answer for the current question
                var selectedAnswer = form["selectedAnswer_" + question.QuesID];

                // Retrieve the course information associated with the question
                var course = db.Courses.FirstOrDefault(c => c.Course_Id == question.CourseID);
                bool isCorrect = false;

                if (!string.IsNullOrEmpty(selectedAnswer))
                {
                    // Check if the selected answer is correct
                    isCorrect = (selectedAnswer == question.Correct_Ans);
                    if (isCorrect)
                    {
                        totalMarks += 2; // Each correct answer gives 2 marks
                    }

                    // Create a new Answer object and save it to the database
                    var answer = new Answer
                    {
                        Ans = selectedAnswer,
                        statuss = isCorrect ? 1 : 0,
                        Quiz_id = question.Quiz_id,
                        CourseID = question.CourseID,
                        CourseName = course?.Course_Name, // Assign CourseName from the retrieved course
                        studentID = studentID,
                        studentName = studentName,
                        QuesID = question.QuesID,
                        Question = question.Question1
                    };

                    db.Answers.Add(answer);
                }

                // Prepare the result data for display
                resultList.Add(new ResultViewModel
                {
                    Question = question.Question1,
                    SelectedAnswer = selectedAnswer,
                    CorrectAnswer = question.Correct_Ans,
                    IsCorrect = isCorrect
                });
            }

            // Save all changes to the database
            db.SaveChanges();

            // Pass the result data and total marks to the view
            ViewBag.TotalMarks = totalMarks;
            ViewBag.FullMarks = fullMarks;
            return View("Result", resultList);
        }

        public ActionResult Index()
        {
            return View(db.Questions.ToList());
        }


        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: Questions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuesID,CourseID,Question1,Ans_A,Ans_B,Ans_C,Ans_D,Correct_Ans,Quiz_id")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(question);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuesID,CourseID,Question1,Ans_A,Ans_B,Ans_C,Ans_D,Correct_Ans,Quiz_id")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
