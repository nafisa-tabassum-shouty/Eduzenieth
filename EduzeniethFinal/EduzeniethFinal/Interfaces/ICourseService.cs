using EduzeniethFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduzeniethFinal.Interfaces
{
    
        public interface ICourseService
        {
            void AddCourse(Course course);
            void UpdateCourse(Course course);
            void DeleteCourse(int id);
            Course GetCourseById(int id);
            IEnumerable<Course> GetAllCourses();
        }
    
}
