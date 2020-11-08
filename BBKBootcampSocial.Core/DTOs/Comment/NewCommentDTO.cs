namespace BBKBootcampSocial.Core.DTOs.Comment
{
    public class NewCommentDTO
    {
        public string Text { get; set; }
        public long PostId { get; set; }
        public long UserId { get; set; }
        public long DestinationUserId { get; set; }
        public string ProfilePic { get; set; }
        public long Id { get; set; }
    }
}
