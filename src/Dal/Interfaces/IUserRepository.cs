using Dal.Models;

namespace Dal.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User> CreateAsync(User entity);
    Task<bool> UpdateAsync(User entity);
    Task<bool> DeleteAsync(User user);
    Task<bool> DeleteByIdAsync(int id);
}