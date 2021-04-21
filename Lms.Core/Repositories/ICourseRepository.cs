using Lms.Api.ResourceParameters;
using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCourses(bool includeModules);
        Task<IEnumerable<Course>> GetAllCourses(CourseResourceParameters courseResourceParameters, bool includeModules);
        Task<Course> GetCourse(int id, bool includeModules);
        Task<Course> GetCourse(int id);
        Task<bool> SaveAsync();
        Task AddAsync<T>(T added);
        void CreateCourse(Course course);
        void DeleteAsync<T>(T removed);
        bool CourseExists(int id);

    }
}
