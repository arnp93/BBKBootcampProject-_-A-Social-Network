using BBKBootcampSocial.Core.DTOs.Account;
using BBKBootcampSocial.Domains.Common_Entities;
using System;

namespace BBKBootcampSocial.Core.DTOs.Notification
{
    public class NotificationDTO
    {
        public long Id { get; set; }
        public long UserOriginId { get; set; }
        public long PostId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Message { get; set; }
        public string ImageName { get; set; }
        public bool IsRead { get; set; }
        public string CreateDate { get; set; }
        public bool IsAccepted { get; set; }
        public long UserDestinationId{ get; set; }
        public TypeOfNotification TypeOfNotification { get; set; }
        public LoginUserInfoDTO User { get; set; }
    }
}
