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
    public class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext db;

        public ModuleRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task AddAsync<T>(T added)
        {
            await db.AddAsync(added);
        }

        public void AddModule(int courseId, Module module)
        {
            if (courseId ==null)
            {
                throw new ArgumentNullException(nameof(courseId));
            }
            if (module is null)
            {
                throw new ArgumentNullException(nameof(module));
            }
            module.CourseId = courseId;
            db.Modules.Add(module);
        }

        public bool CourseExists(int id)
        {
          return  db.Courses.Any(c => c.Id == id);
        }

        public void DeleteAsync<T>(T removed)
        {
            db.Remove(removed);
        }

        public async Task<IEnumerable<Module>> GetAllModules()
        {
            return await db.Modules
                   .Include(c => c.Course).ToListAsync();
        }

        public async Task<Module> GetModule(string title)
        {
            var query = db.Modules
               .Include(m => m.Course);
            return await query.FirstOrDefaultAsync(m => m.Title== title);
        }

        public Module GetModuleForCourse(int id, string title)
        {
            return  db.Modules.Where(m => m.Title == title && m.CourseId == id).FirstOrDefault();
        }

        public Module GetModuleForCourse(int courseid, int id)
        {
            return db.Modules.Where(m => m.Id == id && m.CourseId == courseid).FirstOrDefault();
        }

        public bool ModuleExists(int id)
        {
            return db.Modules.Any(m=>m.Id==id);
        }

        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync() >= 0);
        }

    }
}
