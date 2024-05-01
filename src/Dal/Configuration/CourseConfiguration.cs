using Dal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.Configuration;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public const int NameMaxLength = 50;
    public const int DescriptionMaxLength = 500;

    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(course => course.Id);

        builder.Property(course => course.Name)
               .IsRequired()
               .HasMaxLength(NameMaxLength);
        builder.HasIndex(course => course.Name)
               .IsUnique();

        builder.Property(course => course.Description)
               .HasMaxLength(DescriptionMaxLength);

        builder.Property(course => course.InstructorId);
        builder.HasOne(c => c.Instructor)
               .WithMany(u => u.CoursesAsInstructor)
               .HasForeignKey(c => c.InstructorId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany<User>(course => course.Students)
               .WithMany(user => user.CoursesAsStudent);
    }
}