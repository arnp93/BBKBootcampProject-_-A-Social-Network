using System.Collections.Generic;
using BBKBootcampSocial.Core.DTOs.Post;

namespace BBKBootcampSocial.Core.DTOs.Account
{
    public class LoginUserInfoDTO
    {
        public string Token { get; set; }
        public int ExpireTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePic { get; set; }
        public long UserId { get; set; }
        public IEnumerable<ShowPostDTO> Posts { get; set; }
    }
}
