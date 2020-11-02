using BBKBootcampSocial.Domains.Common;

namespace BBKBootcampSocial.Domains.Post
{
    public class Like : BaseEntity
    {
        public long UserId { get; set; }
        public long PostId { get; set; }
        //public string Type { get; set; }

        #region Relations

        public Post Post { get; set; }

        #endregion
    }
}
