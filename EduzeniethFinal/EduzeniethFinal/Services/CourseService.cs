using EduzeniethFinal.Interfaces;
using EduzeniethFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduzeniethFinal.Services
{
    public class CourseService : ICourseService
    {
        private readonly EduEntities _context;

        public CourseService(EduEntities context)
        {
            _context = context;
        }

        public void AddCourse(Course course)
        {
            course.Created_at = DateTime.Now;
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public void UpdateCourse(Course course)
        {
            _context.Entry(course).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteCourse(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
        }

        public Course GetCourseById(int id)
        {
            return _context.Courses.Find(id);
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return _context.Courses.ToList();
        }
    }
}
