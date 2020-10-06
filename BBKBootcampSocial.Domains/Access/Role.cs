using BBKBootcampSocial.Domains.Common;
using System.Collections.Generic;

namespace BBKBootcampSocial.Domains.Access
{
    public class Role : BaseEntity
    {
        #region Properties

        public long RoleId { get; set; }
        public string Title { get; set; }

        #endregion


        #region Relations

        public virtual ICollection<UserRole> UserRoles { get; set; }

        #endregion
    }
}
