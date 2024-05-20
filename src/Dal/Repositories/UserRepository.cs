using Dal.Interfaces;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public class UserRepository(DataContext context) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }

    public async Task<User> CreateAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        User? existingUser = await context.Users.FindAsync(user.Id);
        if (existingUser is null) return false;
        context.Entry(existingUser).CurrentValues.SetValues(user);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(User user)
    {
        User? entity = await context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
        if (entity is not null) return await DeleteAsync(entity);
        return false;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        User? entity = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is not null) return await DeleteAsync(entity);
        return false;
    }
}