namespace BBKBootcampSocial.Core.DTOs.Account
{
    public class LoginUserInfoDTO
    {
        public string Token { get; set; }
        public int ExpireTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long UserId { get; set; }
    }
}
