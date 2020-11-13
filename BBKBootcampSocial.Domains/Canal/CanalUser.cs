using BBKBootcampSocial.Domains.Common;

namespace BBKBootcampSocial.Domains.Canal
{
    public class CanalUser : BaseEntity
    {
        #region Propertie

        public bool isAdmin { get; set; }
        public long Userid { get; set; }
        public long CanalId { get; set; }

        #endregion

        #region Relations

        public User.User user { get; set; }

        public Canal Canal { get; set; }

        #endregion
    }
}
