using BBKBootcampSocial.Domains.Common;
using BBKBootcampSocial.Domains.Common_Entities;

namespace BBKBootcampSocial.Domains.User
{
    public class Notification : BaseEntity
    {
        public long UserDestinationId { get; set; }
        public long UserOriginId { get; set; }
        public bool IsRead { get; set; }
        public bool IsAccepted { get; set; }
        public TypeOfNotification TypeOfNotification { get; set; }
    }
}
