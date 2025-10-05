using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.UserNotifications
{
    public abstract class UserNotificationCreateDtoBase
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Body { get; set; } = null!;
        public List<Guid> UserProfileIds { get; set; }
        public List<Guid> SitePropertyIds { get; set; }
    }
}