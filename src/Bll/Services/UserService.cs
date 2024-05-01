using System.Security.Cryptography;
using System.Text;
using Bll.Interfaces;
using Bll.Models;
using Dal.Interfaces;
using User = Bll.Models.User;

namespace Bll.Services;

public class UserService(ITokenService tokenService,
    IUserRepository userRepository) : IUserService
{
    public async Task<User> LoginAsync(UserLogin userLogin)
    {
        Dal.Models.User? user = await userRepository.GetByUsernameAsync(userLogin.Username);
        if (user is null) throw new UnauthorizedAccessException("User not found");
        byte[] hash = HashPassword(userLogin.Password, user.PasswordSalt);
        if (!hash.SequenceEqual(user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid password");
        }
        return new User
        {
            Username = user.Username,
            Token = tokenService.CreateToken(user)
        };
    }

    public async Task<User> RegisterAsync(UserRegister userRegister)
    {
        byte[] salt = new HMACSHA512().Key;
        Dal.Models.User u = await userRepository.CreateAsync(
            new Dal.Models.User
            {
                Username = userRegister.Username,
                PasswordHash = HashPassword(userRegister.Password, salt),
                PasswordSalt = salt
            });
        return new User
        {
            Username = u.Username,
            Token = tokenService.CreateToken(u)
        };
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