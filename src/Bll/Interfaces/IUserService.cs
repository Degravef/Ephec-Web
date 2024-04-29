using Bll.Models;

namespace Bll.Interfaces;

public interface IUserService
{
    Task<User> LoginAsync(UserLogin userLogin);
    Task<User> RegisterAsync(UserRegister userRegister);
}