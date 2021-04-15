using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lms.Core.Entities;

namespace Lms.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    builder.Entity<Module>().HasKey(m => new { m.CourseId, m.Id});
        //    //builder.Entity<Course>().HasKey(c => new { c.Id, c.Modules });
        //}

        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
    }
}
