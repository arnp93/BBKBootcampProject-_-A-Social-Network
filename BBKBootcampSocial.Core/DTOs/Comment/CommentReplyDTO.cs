namespace BBKBootcampSocial.Core.DTOs.Comment
{
    public class CommentReplyDTO
    {
        public string Text { get; set; }
        public long PostId { get; set; }
        public long UserId { get; set; }
        public long ParentId { get; set; }
        public long DestinationUserId { get; set; }
        public long Id { get; set; }
    }
}
