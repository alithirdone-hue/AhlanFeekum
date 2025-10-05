using AhlanFeekum.UserProfiles;
using AhlanFeekum.SiteProperties;

using System;
using System.Collections.Generic;

namespace AhlanFeekum.UserNotifications
{
    public abstract class UserNotificationWithNavigationPropertiesBase
    {
        public UserNotification UserNotification { get; set; } = null!;

        

        public List<UserProfile> UserProfiles { get; set; } = null!;
        public List<SiteProperty> SiteProperties { get; set; } = null!;
        
    }
}