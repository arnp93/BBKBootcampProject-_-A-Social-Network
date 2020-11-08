using System;
using System.Collections.Generic;
using BBKBootcampSocial.Core.DTOs.Notification;
using BBKBootcampSocial.Core.DTOs.Post;
using BBKBootcampSocial.Domains.User;

namespace BBKBootcampSocial.Core.DTOs.Account
{
    public class LoginUserInfoDTO
    {
        public string Token { get; set; }
        public int ExpireTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDay { get; set; }
        public User.Gender Gender { get; set; }
        public string About { get; set; }
        public bool IsPrivate { get; set; }
        public string SocialNetwork { get; set; }
        public string ProfilePic { get; set; }
        public string CoverPic { get; set; }
        public long UserId { get; set; }
        public IEnumerable<ShowPostDTO> Posts { get; set; }
        public List<NotificationDTO> Notifications{ get; set; }
        public List<FriendDTO> Friends { get; set; }
    }
}
