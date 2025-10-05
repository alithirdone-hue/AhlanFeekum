using AhlanFeekum.UserProfiles;
using AhlanFeekum.SiteProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace AhlanFeekum.UserNotifications
{
    public abstract class UserNotificationManagerBase : DomainService
    {
        protected IUserNotificationRepository _userNotificationRepository;
        protected IRepository<UserProfile, Guid> _userProfileRepository;
        protected IRepository<SiteProperty, Guid> _sitePropertyRepository;

        public UserNotificationManagerBase(IUserNotificationRepository userNotificationRepository,
        IRepository<UserProfile, Guid> userProfileRepository,
        IRepository<SiteProperty, Guid> sitePropertyRepository)
        {
            _userNotificationRepository = userNotificationRepository;
            _userProfileRepository = userProfileRepository;
            _sitePropertyRepository = sitePropertyRepository;
        }

        public virtual async Task<UserNotification> CreateAsync(
        List<Guid> userProfileIds,
        List<Guid> sitePropertyIds,
        string title, string body)
        {
            Check.NotNullOrWhiteSpace(title, nameof(title));
            Check.NotNullOrWhiteSpace(body, nameof(body));

            var userNotification = new UserNotification(
             GuidGenerator.Create(),
             title, body
             );

            await SetUserProfilesAsync(userNotification, userProfileIds);
            await SetSitePropertiesAsync(userNotification, sitePropertyIds);

            return await _userNotificationRepository.InsertAsync(userNotification);
        }

        public virtual async Task<UserNotification> UpdateAsync(
            Guid id,
            List<Guid> userProfileIds,
        List<Guid> sitePropertyIds,
        string title, string body, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(title, nameof(title));
            Check.NotNullOrWhiteSpace(body, nameof(body));

            var queryable = await _userNotificationRepository.WithDetailsAsync(x => x.UserProfiles, x => x.SiteProperties);
            var query = queryable.Where(x => x.Id == id);

            var userNotification = await AsyncExecuter.FirstOrDefaultAsync(query);

            userNotification.Title = title;
            userNotification.Body = body;

            await SetUserProfilesAsync(userNotification, userProfileIds);
            await SetSitePropertiesAsync(userNotification, sitePropertyIds);

            userNotification.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _userNotificationRepository.UpdateAsync(userNotification);
        }

        private async Task SetUserProfilesAsync(UserNotification userNotification, List<Guid> userProfileIds)
        {
            if (userProfileIds == null || !userProfileIds.Any())
            {
                userNotification.RemoveAllUserProfiles();
                return;
            }

            var query = (await _userProfileRepository.GetQueryableAsync())
                .Where(x => userProfileIds.Contains(x.Id))
                .Select(x => x.Id);

            var userProfileIdsInDb = await AsyncExecuter.ToListAsync(query);
            if (!userProfileIdsInDb.Any())
            {
                return;
            }

            userNotification.RemoveAllUserProfilesExceptGivenIds(userProfileIdsInDb);

            foreach (var userProfileId in userProfileIdsInDb)
            {
                userNotification.AddUserProfile(userProfileId);
            }
        }

        private async Task SetSitePropertiesAsync(UserNotification userNotification, List<Guid> sitePropertyIds)
        {
            if (sitePropertyIds == null || !sitePropertyIds.Any())
            {
                userNotification.RemoveAllSiteProperties();
                return;
            }

            var query = (await _sitePropertyRepository.GetQueryableAsync())
                .Where(x => sitePropertyIds.Contains(x.Id))
                .Select(x => x.Id);

            var sitePropertyIdsInDb = await AsyncExecuter.ToListAsync(query);
            if (!sitePropertyIdsInDb.Any())
            {
                return;
            }

            userNotification.RemoveAllSitePropertiesExceptGivenIds(sitePropertyIdsInDb);

            foreach (var sitePropertyId in sitePropertyIdsInDb)
            {
                userNotification.AddSiteProperty(sitePropertyId);
            }
        }

    }
}