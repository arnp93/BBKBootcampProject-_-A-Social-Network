using BBKBootcampSocial.Domains.Common;
using System.Collections.Generic;
using BBKBootcampSocial.Domains.Access;

namespace BBKBootcampSocial.Domains.User
{
    public class User : BaseEntity
    {
        #region Properties

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Facebook { get; set; }
        public string LinkdIn { get; set; }
        public string Instagram { get; set; }
        public string WhatsApp { get; set; }
        public bool IsActive { get; set; }


        #endregion

        #region Relations

        public virtual ICollection<Post.Post> Posts { get; set; }
        public virtual ICollection<Post.Story> Stories { get; set; }
        public virtual ICollection<Image.Image> Images { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

        #endregion

    }
}
