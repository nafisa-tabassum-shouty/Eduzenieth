using EduzeniethFinal.Interfaces;
using EduzeniethFinal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EduzeniethFinal.Services
{
    public class AdminService : IAdminService
    {
        private readonly EduEntities _context;

        public AdminService(EduEntities context)
        {
            _context = context;
        }

        // Student methods
        public IEnumerable<Student> GetStudents()
        {
            return _context.Students.ToList();
        }

        public Student GetStudentById(int id)
        {
            return _context.Students.Find(id);
        }

        public void AddStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void EditStudent(Student student)
        {
            _context.Entry(student).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Student> GetPendingStudents()
        {
            return _context.Students.Where(s => s.Status == 0).ToList();
        }

        public void AcceptPendingStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                student.Status = 1;
                _context.SaveChanges();
            }
        }

        public void DeclinePendingStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }

        // Teacher methods
        public IEnumerable<Teacher> GetTeachers()
        {
            return _context.Teachers.ToList();
        }

        public Teacher GetTeacherById(int id)
        {
            return _context.Teachers.Find(id);
        }

        public void AddTeacher(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            _context.SaveChanges();
        }

        public void EditTeacher(Teacher teacher)
        {
            _context.Entry(teacher).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteTeacher(int id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Teacher> GetPendingTeachers()
        {
            return _context.Teachers.Where(t => t.Status == 0).ToList();
        }

        public void AcceptPendingTeacher(int id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher != null)
            {
                teacher.Status = 1;
                _context.SaveChanges();
            }
        }

        public void DeclinePendingTeacher(int id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                _context.SaveChanges();
            }
        }

        // Course methods
        public IEnumerable<Course> GetCourses()
        {
            return _context.Courses.ToList();
        }

        public void AddCourse(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }
    }


}