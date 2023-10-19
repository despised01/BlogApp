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
    public class RoleRepository : IRoleRepository
    {
        private readonly BlogDB _db;

        public RoleRepository(BlogDB db)
        {
            _db = db;
        }

        public async Task Create(Role item)
        {
            Guid roleId = Guid.NewGuid();
            item.Id = roleId;
            var entry = _db.Entry(item);
            if (entry.State == EntityState.Detached)
                _db.Roles.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Role item)
        {
            _db.Roles.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<Role> Get(Guid id)
        {
            return await _db.Roles
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _db.Roles
                .ToListAsync();
        }

        public async Task<Role> GetByName(string name)
        {
            return await _db.Roles.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task Update(Role item)
        {
            var oldItem = Get(item.Id);

            if (!string.IsNullOrEmpty(item.Name))
                oldItem.Result.Name = item.Name;
            if (item.Description != null)
                oldItem.Result.Description = item.Description;

            var entry = _db.Entry(oldItem.Result);

            if (entry.State == EntityState.Detached)
                _db.Roles.Update(item);
            await _db.SaveChangesAsync();

        }
    }
}
