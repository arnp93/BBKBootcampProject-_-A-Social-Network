namespace BBKBootcampSocial.Core.DTOs.Post
{
    public class LikeDTO
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePic { get; set; }
        public bool IsDelete { get; set; }
    }
}
