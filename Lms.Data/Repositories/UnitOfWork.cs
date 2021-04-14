using Lms.Api.Data;
using Lms.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
  public  class UnitOfWork : IUnitOfwork
    {
        private readonly ApplicationDbContext db;
        
        public ICourseRepository CourseRepository { get; private set; }
        public IModuleRepository ModuleRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            CourseRepository = new CourseRepository(db);
            ModuleRepository = new ModuleRepository(db);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}

