using BlogApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DLL.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> Get(Guid id);
        Task<User> GetByLogin(string login);
        Task Create(User item);
        Task Update(User item);
        Task Delete(User item);
    }
}
