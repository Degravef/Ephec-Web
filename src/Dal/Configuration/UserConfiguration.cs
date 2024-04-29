using Dal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Username)
               .IsRequired()
               .HasMaxLength(50);
        builder.Property(user => user.PasswordHash)
               .IsRequired();
        builder.Property(user => user.PasswordSalt)
               .IsRequired();
        builder.Property(user => user.Role);
        builder.HasIndex(user => user.Username)
               .IsUnique();
    }
}