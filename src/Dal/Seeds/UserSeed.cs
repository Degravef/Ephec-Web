using System.Security.Cryptography;
using System.Text;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Seeds;

public static class UserSeed
{
    public static void SeedUser(this ModelBuilder modelBuilder)
    {
        byte[] salt1 = new HMACSHA512().Key;
        byte[] salt2 = new HMACSHA512().Key;
        byte[] salt3 = new HMACSHA512().Key;
        modelBuilder.Entity<User>()
                    .HasData(
                        new User
                        {
                            Id = 1,
                            Username = "admin",
                            PasswordSalt = salt1,
                            PasswordHash = HashPassword(password: "123", salt1),
                            Role = Role.Admin
                        },
                        new User
                        {
                            Id = 2,
                            Username = "Fievez",
                            PasswordSalt = salt2,
                            PasswordHash = HashPassword(password: "vfi", salt2),
                            Role = Role.Instructor
                        },
                        new User
                        {
                            Id = 3,
                            Username = "François",
                            PasswordSalt = salt3,
                            PasswordHash = HashPassword(password: "fdg", salt3),
                            Role = Role.Student
                        }
                    );
    }


    private static byte[] HashPassword(string password, byte[] salt)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            password: Encoding.UTF8.GetBytes(password),
            salt: Encoding.UTF8.GetBytes(Convert.ToHexString(salt)),
            iterations: 10000,
            hashAlgorithm: HashAlgorithmName.SHA512,
            outputLength: 512
        );
    }
}