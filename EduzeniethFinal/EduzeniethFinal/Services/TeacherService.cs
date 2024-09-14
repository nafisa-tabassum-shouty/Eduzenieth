using EduzeniethFinal.Interfaces;
using EduzeniethFinal.Models;
using System.Collections.Generic;
using System.Linq;

namespace EduzeniethFinal.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly EduEntities _context;

        public TeacherService(EduEntities context)
        {
            _context = context;
        }

        public void AddTeacher(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            _context.SaveChanges();
        }

        public void UpdateTeacher(Teacher teacher)
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

        public Teacher GetTeacherById(int id)
        {
            return _context.Teachers.Find(id);
        }

        public IEnumerable<Teacher> GetAllTeachers()
        {
            return _context.Teachers.ToList();
        }

        public IEnumerable<Teacher> GetPendingTeachers()
        {
            return _context.Teachers.Where(t => t.Status == 0).ToList();
        }

        public void AcceptTeacherRegistration(int id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher != null)
            {
                teacher.Status = 1;
                _context.SaveChanges();
            }
        }

        public void DeclineTeacherRegistration(int id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                _context.SaveChanges();
            }
        }
    }
}
