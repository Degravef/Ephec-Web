using Dal.Models;

namespace Dal.Interfaces;

public interface IUserRepository
{
    Task<User?> GetById(int id);
    Task<User?> GetByUsername(string username);
    Task<User> Create(User user);
    Task<User> Update(User user);
    Task Delete(int id);
}