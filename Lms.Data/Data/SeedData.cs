using Bogus;
using Lms.Api.Data;
using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class SeedData
    {
        public static async Task InitializeAcync(IServiceProvider services)
        {
            using (var context = new ApplicationDbContext(services.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Courses.Any())
                {
                    return;
                }
                var fake = new Faker("sv");

                var courses = new List<Course>();

                for (int i = 0; i < 20; i++)
                {
                    var courseClass = new Course
                    {
                        Title = fake.Company.CatchPhrase(),
                        StartDate = DateTime.Now.AddDays(fake.Random.Int(-2, 2))
                    };
                   courses.Add(courseClass);
                }
                await context.AddRangeAsync(courses);
                var modulers = new List<Module>();
                foreach (var course in courses)
                {

                    var module = new Module
                    {
                        Title = fake.Company.CatchPhrase(),
                        Course=course,
                        StartDate = DateTime.Now.AddDays(fake.Random.Int(-2, 2)),

                    };
                    modulers.Add(module);
                } 
                
                await context.AddRangeAsync(modulers);
                await context.SaveChangesAsync();

            }
        }
    }
}

