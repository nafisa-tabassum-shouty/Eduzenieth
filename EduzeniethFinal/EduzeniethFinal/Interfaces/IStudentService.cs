using EduzeniethFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduzeniethFinal.Interfaces
{
    public interface IStudentService
    {
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
        Student GetStudentById(int id);
        IEnumerable<Student> GetAllStudents();
        IEnumerable<Student> GetPendingStudents();
        void AcceptStudentRegistration(int id);
        void DeclineStudentRegistration(int id);
    }
}
