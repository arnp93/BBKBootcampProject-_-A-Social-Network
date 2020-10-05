using BBKBootcampSocial.Domains.Common;

namespace BBKBootcampSocial.Domains.Post
{
    public class Story : BaseEntity
    {
        #region Properties

        public string FileName { get; set; }
        public string Text { get; set; }
        public long UserId { get; set; }

        #endregion

        #region Relations

        public virtual User.User User { get; set; }

        #endregion

    }
}
