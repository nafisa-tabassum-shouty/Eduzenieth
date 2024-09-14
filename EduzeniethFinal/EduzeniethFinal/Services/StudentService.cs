using EduzeniethFinal.Interfaces;
using EduzeniethFinal.Models;
using System.Collections.Generic;
using System.Linq;

namespace EduzeniethFinal.Services
{
    public class StudentService : IStudentService
    {
        private readonly EduEntities _context;

        public StudentService(EduEntities context)
        {
            _context = context;
        }

        public void AddStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void UpdateStudent(Student student)
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

        public Student GetStudentById(int id)
        {
            return _context.Students.Find(id);
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _context.Students.ToList();
        }

        public IEnumerable<Student> GetPendingStudents()
        {
            return _context.Students.Where(s => s.Status == 0).ToList();
        }

        public void AcceptStudentRegistration(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                student.Status = 1;
                _context.SaveChanges();
            }
        }

        public void DeclineStudentRegistration(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }
    }
}
