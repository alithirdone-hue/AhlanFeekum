using System;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.UserNotifications
{
    public class UserNotificationUserProfile : Entity
    {

        public Guid UserNotificationId { get; protected set; }

        public Guid UserProfileId { get; protected set; }

        private UserNotificationUserProfile()
        {

        }

        public UserNotificationUserProfile(Guid userNotificationId, Guid userProfileId)
        {
            UserNotificationId = userNotificationId;
            UserProfileId = userProfileId;
        }

        public override object[] GetKeys()
        {
            return new object[]
                {
                    UserNotificationId,
                    UserProfileId
                };
        }
    }
}