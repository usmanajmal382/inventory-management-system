using inventory.application.Interfaces;
using inventory.core.Models;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _ctx;

        public UserRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _ctx.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _ctx.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _ctx.Users.OrderBy(u => u.FullName).ToListAsync();

        public async Task<IEnumerable<User>> GetActiveUsersAsync() =>
            await _ctx.Users.Where(u => u.IsActive).OrderBy(u => u.FullName).ToListAsync();

        public async Task<User> AddAsync(User user)
        {
            _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _ctx.Users.Update(user);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _ctx.Users.FindAsync(id);
            if (user != null)
            {
                _ctx.Users.Remove(user);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id) =>
            await _ctx.Users.AnyAsync(u => u.Id == id);

        public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
        {
            var query = _ctx.Users.Where(u => u.Email == email);
            if (excludeId.HasValue)
                query = query.Where(u => u.Id != excludeId.Value);
            return await query.AnyAsync();
        }
    }
}
