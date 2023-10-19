using BlogApp.DLL.Context;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DLL.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogDB _db;

        public PostRepository(BlogDB db)
        {
            _db = db;
        }

        public async Task Create(Post item)
        {
            var entry = _db.Entry(item);
            if (entry.State == EntityState.Detached)
                _db.Posts.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Post item)
        {
            _db.Posts.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<Post> Get(Guid id)
        {
            return await _db.Posts
                .Include(x => x.Comments)
                .Include(x => x.Tags)
                .Include(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            return await _db.Posts
                .Include(a => a.Tags)
                .Include(x => x.Comments)
                .Include(x => x.Author)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllByAuthorId(Guid id)
        {
            return await _db.Posts.Where(x => x.Author_Id == id).ToListAsync();
        }

        public async Task Update(Post item)
        {
            var oldItem = Get(item.Id);

            if (!string.IsNullOrEmpty(item.Title))
                oldItem.Result.Title = item.Title;
            if (!string.IsNullOrEmpty(item.BodyText))
                oldItem.Result.BodyText = item.BodyText;

            var entry = _db.Entry(oldItem.Result);

            if (entry.State == EntityState.Detached)
                _db.Posts.Update(item);
            await _db.SaveChangesAsync();
        }
    }
}
