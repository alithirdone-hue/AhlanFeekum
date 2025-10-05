using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.UserNotifications
{
    public abstract class UserNotificationUpdateDtoBase : IHasConcurrencyStamp
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Body { get; set; } = null!;
        public List<Guid> UserProfileIds { get; set; }
        public List<Guid> SitePropertyIds { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}