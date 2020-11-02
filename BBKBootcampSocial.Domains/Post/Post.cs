using BBKBootcampSocial.Domains.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BBKBootcampSocial.Domains.Post
{
    public class Post : BaseEntity
    {
        #region Properties  
        public string PostText { get; set; }
        public string FileName { get; set; }
        public int TimesOfReports { get; set; }
        public string ReportDeleteReason { get; set; }  
        public long UserId { get; set; }
        public long? CanalId { get; set; }
        #endregion

        #region Relations

        public virtual User.User User { get; set; }
        public virtual ICollection<Comment.Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        [ForeignKey("CanalId")]
        public Canal.Canal Canal { get; set; }

        #endregion

    }
}
