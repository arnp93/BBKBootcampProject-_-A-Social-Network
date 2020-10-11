using BBKBootcampSocial.Domains.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BBKBootcampSocial.Domains.Comment
{
    public class Comment : BaseEntity
    {
        #region Properties

        public string Text { get; set; }
        public int LikeCount { get; set; }

        public long PostId { get; set; }
        public long UserId { get; set; }

        public long? ParentId { get; set; }
        #endregion

        #region Relations

        public virtual Post.Post Post { get; set; }
        [ForeignKey("ParentId")]
        public virtual ICollection<Comment> Replies { get; set; }


        #endregion

    }
}
