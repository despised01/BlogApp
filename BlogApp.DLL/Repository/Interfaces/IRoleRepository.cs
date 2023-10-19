using BlogApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DLL.Repository.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAll();
        Task<Role> Get(Guid id);
        Task Create(Role item);
        Task Update(Role item);
        Task Delete(Role item);
        Task<Role> GetByName(string name);
    }
}
