using HranitelPRO.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HranitelPRO.API.Services
{
    public class UserService : IUserService
    {
        private readonly HranitelContext _context;
        public UserService(HranitelContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> CreateAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(int id, User updatedUser)
        {
            var existing = await _context.Users.FindAsync(id);
            if (existing == null) return null;
            existing.FullName = updatedUser.FullName;
            existing.Email = updatedUser.Email;
            existing.RoleId = updatedUser.RoleId;
            existing.EmployeeId = updatedUser.EmployeeId;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
