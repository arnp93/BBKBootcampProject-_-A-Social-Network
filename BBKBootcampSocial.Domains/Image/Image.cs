using BBKBootcampSocial.Domains.Common;

namespace BBKBootcampSocial.Domains.Image
{
    public class Image : BaseEntity
    {
        #region Properties
        public string ImageName { get; set; }
        public long UserId { get; set; }

        #endregion

        #region Relations

        public virtual User.User User { get; set; }

        #endregion
    }
}
