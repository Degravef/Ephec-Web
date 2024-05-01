using Dal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public const int NameMaxLength = 50;

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Username)
               .IsRequired()
               .HasMaxLength(NameMaxLength);
        builder.HasIndex(user => user.Username)
               .IsUnique();

        builder.Property(user => user.PasswordHash)
               .IsRequired();

        builder.Property(user => user.PasswordSalt)
               .IsRequired();

        builder.Property(user => user.Role);

        builder.HasMany<Course>(user => user.CoursesAsInstructor)
               .WithOne(course => course.Instructor)
               .HasForeignKey(course => course.InstructorId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany<Course>(user => user.CoursesAsStudent)
               .WithMany(course => course.Students);
    }
}