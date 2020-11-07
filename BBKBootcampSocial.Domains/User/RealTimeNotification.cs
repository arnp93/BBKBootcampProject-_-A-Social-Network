using BBKBootcampSocial.Domains.Common;

namespace BBKBootcampSocial.Domains.User
{
    public class RealTimeNotification : BaseEntity
    {
        public long UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}
