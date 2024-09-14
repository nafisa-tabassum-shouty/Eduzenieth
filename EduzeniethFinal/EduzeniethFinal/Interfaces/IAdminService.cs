using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduzeniethFinal.Models;
using System.Collections.Generic;
namespace EduzeniethFinal.Interfaces
{
    

    public interface IAdminService
    {
        IEnumerable<Student> GetStudents();
        Student GetStudentById(int id);
        void AddStudent(Student student);
        void EditStudent(Student student);
        void DeleteStudent(int id);

        IEnumerable<Teacher> GetTeachers();
        Teacher GetTeacherById(int id);
        void AddTeacher(Teacher teacher);
        void EditTeacher(Teacher teacher);
        void DeleteTeacher(int id);

        IEnumerable<Student> GetPendingStudents();
        void AcceptPendingStudent(int id);
        void DeclinePendingStudent(int id);

        IEnumerable<Teacher> GetPendingTeachers();
        void AcceptPendingTeacher(int id);
        void DeclinePendingTeacher(int id);

        void AddCourse(Course course);
        IEnumerable<Course> GetCourses();
    }

}
