using AhlanFeekum.UserProfiles;
using AhlanFeekum.SiteProperties;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace AhlanFeekum.UserNotifications
{
    public abstract class UserNotificationWithNavigationPropertiesDtoBase
    {
        public UserNotificationDto UserNotification { get; set; } = null!;

        public List<UserProfileDto> UserProfiles { get; set; } = new List<UserProfileDto>();
        public List<SitePropertyDto> SiteProperties { get; set; } = new List<SitePropertyDto>();

    }
}