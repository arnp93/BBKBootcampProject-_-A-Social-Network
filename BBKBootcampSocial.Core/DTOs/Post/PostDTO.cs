namespace BBKBootcampSocial.Core.DTOs.Post
{
    public class PostDTO
    {
        public string PostText { get; set; }
        public string FileName { get; set; }
        public long UserId { get; set; }
        public long? CanalId { get; set; }
    }
}
