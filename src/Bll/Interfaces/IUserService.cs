using Bll.Models;
using Dal.Models;
using User = Bll.Models.User;

namespace Bll.Interfaces;

public interface IUserService
{
    Task<User> LoginAsync(UserLogin userLogin);
    Task<User> RegisterAsync(UserRegister userRegister);
    Task<bool> PromoteAsync(UserPromoteDto userPromoteDto);
    Task<IEnumerable<UserListDto>> GetAllAsync();
    Task<bool> DeleteByIdAsync(int id);
    Task<Role?> GetRoleAsync(int userId);
}