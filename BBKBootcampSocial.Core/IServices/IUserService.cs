using System.Threading.Tasks;
using BBKBootcampSocial.Core.DTOs.Account;
using BBKBootcampSocial.Domains.User;

namespace BBKBootcampSocial.Core.IServices
{
    public interface IUserService
    {
        Task<RegisterUserResult> AddUser(RegisterUserDTO user);

        Task<bool> IsEmailExist(string email);
        Task<bool> IsUserExist(string email, string password);

        Task<LoginUserResult> LoginUser(LoginUserDTO login);

        Task<bool> ValidateEmailAndPassword(string email, string password);

        Task<User> GetUserByEmail(string email);

        Task<User> GetUserById(long id);

        Task<bool> IsUserActive(string email);
        Task<bool> ActiveAccount(string activeCode);
    }
}
