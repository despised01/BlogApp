using BlogApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DLL.Repository.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAll();
        Task<Post> Get(Guid id);
        Task Create(Post item);
        Task Update(Post item);
        Task Delete(Post item);
        Task<IEnumerable<Post>> GetAllByAuthorId(Guid authorGuid);
    }
}
