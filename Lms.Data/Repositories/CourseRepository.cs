using Lms.Api.Data;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
   public  class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext db;
        public CourseRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public async Task AddAsync<T>(T added)
        {
          await  db.AddAsync(added);
        }

        public  bool CourseExists(int id)
        {
            return db.Courses.Any(c=>c.Id==id);  
        }

        public void DeleteAsync<T>(T removed)
        {
            db.Remove(removed);
        }

        public async Task<IEnumerable<Course>> GetAllCourses(bool includeModules)
        {
            return includeModules ? await db.Courses
                   .Include(c => c.Modules).ToListAsync() : 
                   await db.Courses.ToListAsync();
        }

        public async Task<Course> GetCourse(int id, bool includeModules)
        {
            var query = db.Courses
                 .AsQueryable();
            if (includeModules)
            {
                query = query.Include(c => c.Modules);
            }
            return await query.FirstOrDefaultAsync(c=>c.Id==id);
        }

        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }
    }
}
