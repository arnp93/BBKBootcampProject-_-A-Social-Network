using BBKBootcampSocial.Core.DTOs.Account;
using BBKBootcampSocial.Domains.Common_Entities;
using System;

namespace BBKBootcampSocial.Core.DTOs.Notification
{
    public class NotificationDTO
    {
        public long UserOriginId { get; set; }
        public bool IsRead { get; set; }
        public string CreateDate { get; set; }
        public TypeOfNotification TypeOfNotification { get; set; }
        public LoginUserInfoDTO User { get; set; }
    }
}
