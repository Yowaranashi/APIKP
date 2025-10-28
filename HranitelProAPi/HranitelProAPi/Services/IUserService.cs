using HranitelPRO.API.Models;

namespace HranitelPRO.API.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(User user);
        Task<User?> UpdateAsync(int id, User updatedUser);
        Task<bool> DeleteAsync(int id);
    }
}
