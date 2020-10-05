using BBKBootcampSocial.Domains.Common;
using System.Collections.Generic;

namespace BBKBootcampSocial.Domains.Post
{
    public class Post : BaseEntity
    {
        #region Properties  
        public string PostText { get; set; }
        public string FileName { get; set; }

        public long UserId { get; set; }
        #endregion

        #region Relations

        public virtual User.User User { get; set; }

        public virtual ICollection<Comment.Comment> Comments { get; set; }

        #endregion

    }
}
