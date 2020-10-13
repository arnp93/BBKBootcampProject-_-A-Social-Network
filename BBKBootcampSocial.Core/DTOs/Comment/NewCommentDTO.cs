namespace BBKBootcampSocial.Core.DTOs.Comment
{
    public class NewCommentDTO
    {
        public string Text { get; set; }
        public long PostId { get; set; }
        public long UserId { get; set; }
    }
}
