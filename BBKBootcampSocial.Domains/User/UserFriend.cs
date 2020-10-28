using BBKBootcampSocial.Domains.Common;

namespace BBKBootcampSocial.Domains.User
{
    public class UserFriend : BaseEntity
    {
        public long FriendUserId { get; set; }
        public long UserId { get; set; }

        #region Relations

        public User User { get; set; }

        #endregion
    }
}
