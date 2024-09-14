using EduzeniethFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduzeniethFinal.Interfaces
{
    public interface ITeacherService
    {
        void AddTeacher(Teacher teacher);
        void UpdateTeacher(Teacher teacher);
        void DeleteTeacher(int id);
        Teacher GetTeacherById(int id);
        IEnumerable<Teacher> GetAllTeachers();
        IEnumerable<Teacher> GetPendingTeachers();
        void AcceptTeacherRegistration(int id);
        void DeclineTeacherRegistration(int id);
    }
}
