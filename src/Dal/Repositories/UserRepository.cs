using Dal.Interfaces;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public class UserRepository(DataContext context) : IUserRepository
{
    public async Task<User?> GetById(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<User?> GetByUsername(string username)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> Create(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User> Update(User user)
    {
        User? existingUser = await context.Users.FindAsync(user.Id);
        if (existingUser is null) return user;
        context.Entry(existingUser).CurrentValues.SetValues(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task Delete(int id)
    {
        User? existingUser = await context.Users.FindAsync(id);
        if (existingUser is null) return;
        context.Users.Remove(existingUser);
        await context.SaveChangesAsync();
    }
}