using System.Threading.Tasks;
using BBKBootcampSocial.Core.DTOs.Account;

namespace BBKBootcampSocial.Core.IServices
{
    public interface IUserService
    {
        Task<RegisterUserResult> AddUser(RegisterUserDTO user);

        Task<bool>isEmailExist(string email);
    }
}
