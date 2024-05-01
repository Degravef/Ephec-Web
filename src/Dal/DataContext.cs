using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }

    public required DbSet<User> Users { get; init; }
    public required DbSet<Course> Courses { get; init; }
}