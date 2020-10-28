using System.Collections.Generic;
using System.Threading.Tasks;
using BBKBootcampSocial.Core.DTOs.Account;
using BBKBootcampSocial.Core.DTOs.Notification;
using BBKBootcampSocial.Domains.User;

namespace BBKBootcampSocial.Core.AllServices.IServices
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
        Task<LoginUserInfoDTO> ReturnUserByIdWithPosts(long userId);
        Task<bool> AddFriend(long userId,long currentUserId);
        Task<List<NotificationDTO>> GetNotificationsOfUser(long userId);
        Task AcceptFriend(long currentUserId, long originUserId);
        Task<List<FriendDTO>> GetFriendListByUserId(long userId);
        Task DeleteNotification(long notificationId);
        Task<List<LoginUserInfoDTO>> GetLatestUsers();
    }
}
