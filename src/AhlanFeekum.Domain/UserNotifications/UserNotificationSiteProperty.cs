using System;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.UserNotifications
{
    public class UserNotificationSiteProperty : Entity
    {

        public Guid UserNotificationId { get; protected set; }

        public Guid SitePropertyId { get; protected set; }

        private UserNotificationSiteProperty()
        {

        }

        public UserNotificationSiteProperty(Guid userNotificationId, Guid sitePropertyId)
        {
            UserNotificationId = userNotificationId;
            SitePropertyId = sitePropertyId;
        }

        public override object[] GetKeys()
        {
            return new object[]
                {
                    UserNotificationId,
                    SitePropertyId
                };
        }
    }
}