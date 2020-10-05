using BBKBootcampSocial.Domains.Common;

namespace BBKBootcampSocial.Domains.Comment
{
    public class Comment : BaseEntity
    {
        #region Properties

        public string Text { get; set; }
        public int LikeCount { get; set; }

        public long PostId { get; set; }

        #endregion

        #region Relations

        public virtual Post.Post Post { get; set; }

        #endregion

    }
}
