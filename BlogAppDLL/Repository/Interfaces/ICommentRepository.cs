using BlogApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DLL.Repository.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAll();
        Task<Comment> Get(Guid id);
        Task Create(Comment item);
        Task Update(Comment item);
        Task Delete(Comment item);
    }
}
