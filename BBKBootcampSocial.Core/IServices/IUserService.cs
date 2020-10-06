using System.Threading.Tasks;
using BBKBootcampSocial.Domains.User;

namespace BBKBootcampSocial.Core.IServices
{
    public interface IUserService
    {
        Task AddUser(User user);
    }
}
