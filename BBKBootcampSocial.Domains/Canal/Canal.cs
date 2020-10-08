using BBKBootcampSocial.Domains.Common;
using System.Collections.Generic;

namespace BBKBootcampSocial.Domains.Canal
{
    public class Canal : BaseEntity
    {
        #region Properties
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }

        #endregion

        #region Relations

        public virtual ICollection<Post.Post> Posts { get; set; }

        public ICollection<CanalUser> Users { get; set; }

        #endregion
    }
}
