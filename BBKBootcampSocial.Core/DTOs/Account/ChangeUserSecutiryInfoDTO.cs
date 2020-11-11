namespace BBKBootcampSocial.Core.DTOs.Account
{
    public class ChangeUserSecutiryInfoDTO
    {
        public string Password { get; set; }
        public string RePassword { get; set; }
        public bool isPrivate { get; set; }
    }
}
