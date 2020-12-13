using BBKBootcampSocial.Domains.Common;

namespace BBKBootcampSocial.Domains.Access
{
    public class UserRole : BaseEntity
    {
        #region Properties

        public long UserId { get; set; }

        public long RoleId { get; set; }

        #endregion

        #region Relations

        public virtual User.User User { get; set; }

        public virtual Role Role { get; set; }

        #endregion
    }
}
