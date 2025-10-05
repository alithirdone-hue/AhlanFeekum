using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace AhlanFeekum.UserNotifications
{
    public abstract class UserNotificationBase : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Title { get; set; }

        [NotNull]
        public virtual string Body { get; set; }

        public ICollection<UserNotificationUserProfile> UserProfiles { get; private set; }
        public ICollection<UserNotificationSiteProperty> SiteProperties { get; private set; }

        protected UserNotificationBase()
        {

        }

        public UserNotificationBase(Guid id, string title, string body)
        {

            Id = id;
            Check.NotNull(title, nameof(title));
            Check.NotNull(body, nameof(body));
            Title = title;
            Body = body;
            UserProfiles = new Collection<UserNotificationUserProfile>();
            SiteProperties = new Collection<UserNotificationSiteProperty>();
        }
        public virtual void AddUserProfile(Guid userProfileId)
        {
            Check.NotNull(userProfileId, nameof(userProfileId));

            if (IsInUserProfiles(userProfileId))
            {
                return;
            }

            UserProfiles.Add(new UserNotificationUserProfile(Id, userProfileId));
        }

        public virtual void RemoveUserProfile(Guid userProfileId)
        {
            Check.NotNull(userProfileId, nameof(userProfileId));

            if (!IsInUserProfiles(userProfileId))
            {
                return;
            }

            UserProfiles.RemoveAll(x => x.UserProfileId == userProfileId);
        }

        public virtual void RemoveAllUserProfilesExceptGivenIds(List<Guid> userProfileIds)
        {
            Check.NotNullOrEmpty(userProfileIds, nameof(userProfileIds));

            UserProfiles.RemoveAll(x => !userProfileIds.Contains(x.UserProfileId));
        }

        public virtual void RemoveAllUserProfiles()
        {
            UserProfiles.RemoveAll(x => x.UserNotificationId == Id);
        }

        private bool IsInUserProfiles(Guid userProfileId)
        {
            return UserProfiles.Any(x => x.UserProfileId == userProfileId);
        }

        public virtual void AddSiteProperty(Guid sitePropertyId)
        {
            Check.NotNull(sitePropertyId, nameof(sitePropertyId));

            if (IsInSiteProperties(sitePropertyId))
            {
                return;
            }

            SiteProperties.Add(new UserNotificationSiteProperty(Id, sitePropertyId));
        }

        public virtual void RemoveSiteProperty(Guid sitePropertyId)
        {
            Check.NotNull(sitePropertyId, nameof(sitePropertyId));

            if (!IsInSiteProperties(sitePropertyId))
            {
                return;
            }

            SiteProperties.RemoveAll(x => x.SitePropertyId == sitePropertyId);
        }

        public virtual void RemoveAllSitePropertiesExceptGivenIds(List<Guid> sitePropertyIds)
        {
            Check.NotNullOrEmpty(sitePropertyIds, nameof(sitePropertyIds));

            SiteProperties.RemoveAll(x => !sitePropertyIds.Contains(x.SitePropertyId));
        }

        public virtual void RemoveAllSiteProperties()
        {
            SiteProperties.RemoveAll(x => x.UserNotificationId == Id);
        }

        private bool IsInSiteProperties(Guid sitePropertyId)
        {
            return SiteProperties.Any(x => x.SitePropertyId == sitePropertyId);
        }
    }
}