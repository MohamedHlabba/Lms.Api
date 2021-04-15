using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetAllModules();
        Module GetModuleForCourse(int id,string title);
        Task<Module> GetModule(string title);
        Task<bool> SaveAsync();
        Task AddAsync<T>(T added);
        void DeleteAsync<T>(T removed);
        bool ModuleExists(int id);

    }
}
