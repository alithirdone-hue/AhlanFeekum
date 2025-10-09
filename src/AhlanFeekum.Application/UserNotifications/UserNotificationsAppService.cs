using AhlanFeekum.Shared;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.UserProfiles;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AhlanFeekum.Permissions;
using AhlanFeekum.UserNotifications;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using AhlanFeekum.Shared;
using AhlanFeekum.FireBase;

namespace AhlanFeekum.UserNotifications
{

    [Authorize(AhlanFeekumPermissions.UserNotifications.Default)]
    public abstract class UserNotificationsAppServiceBase : AhlanFeekumAppService
    {
        protected IDistributedCache<UserNotificationDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IUserNotificationRepository _userNotificationRepository;
        protected UserNotificationManager _userNotificationManager;

        protected IRepository<AhlanFeekum.UserProfiles.UserProfile, Guid> _userProfileRepository;
        protected IRepository<AhlanFeekum.SiteProperties.SiteProperty, Guid> _sitePropertyRepository;

        
        public UserNotificationsAppServiceBase(IUserNotificationRepository userNotificationRepository, UserNotificationManager userNotificationManager, IDistributedCache<UserNotificationDownloadTokenCacheItem, string> downloadTokenCache, IRepository<AhlanFeekum.UserProfiles.UserProfile, Guid> userProfileRepository, IRepository<AhlanFeekum.SiteProperties.SiteProperty, Guid> sitePropertyRepository)
        {
            _downloadTokenCache = downloadTokenCache;
            _userNotificationRepository = userNotificationRepository;
            _userNotificationManager = userNotificationManager; _userProfileRepository = userProfileRepository;
            _sitePropertyRepository = sitePropertyRepository;
        }

        public virtual async Task<PagedResultDto<UserNotificationWithNavigationPropertiesDto>> GetListAsync(GetUserNotificationsInput input)
        {
            var totalCount = await _userNotificationRepository.GetCountAsync(input.FilterText, input.Title, input.Body, input.UserProfileId, input.SitePropertyId);
            var items = await _userNotificationRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Title, input.Body, input.UserProfileId, input.SitePropertyId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<UserNotificationWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<UserNotificationWithNavigationProperties>, List<UserNotificationWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<UserNotificationWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<UserNotificationWithNavigationProperties, UserNotificationWithNavigationPropertiesDto>
                (await _userNotificationRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<UserNotificationDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<UserNotification, UserNotificationDto>(await _userNotificationRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetUserProfileLookupAsync(LookupRequestDto input)
        {
            var query = (await _userProfileRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Name != null &&
                         x.Name.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<AhlanFeekum.UserProfiles.UserProfile>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<AhlanFeekum.UserProfiles.UserProfile>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetSitePropertyLookupAsync(LookupRequestDto input)
        {
            var query = (await _sitePropertyRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.PropertyTitle != null &&
                         x.PropertyTitle.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<AhlanFeekum.SiteProperties.SiteProperty>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<AhlanFeekum.SiteProperties.SiteProperty>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        [Authorize(AhlanFeekumPermissions.UserNotifications.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _userNotificationRepository.DeleteAsync(id);
        }

        [Authorize(AhlanFeekumPermissions.UserNotifications.Create)]
        public virtual async Task<UserNotificationDto> CreateAsync(UserNotificationCreateDto input)
        {

            var userNotification = await _userNotificationManager.CreateAsync(
            input.UserProfileIds, input.SitePropertyIds, input.Title, input.Body
            );


            return ObjectMapper.Map<UserNotification, UserNotificationDto>(userNotification);
        }

        [Authorize(AhlanFeekumPermissions.UserNotifications.Edit)]
        public virtual async Task<UserNotificationDto> UpdateAsync(Guid id, UserNotificationUpdateDto input)
        {

            var userNotification = await _userNotificationManager.UpdateAsync(
            id,
            input.UserProfileIds, input.SitePropertyIds, input.Title, input.Body, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<UserNotification, UserNotificationDto>(userNotification);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(UserNotificationExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var userNotifications = await _userNotificationRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Title, input.Body, input.UserProfileId, input.SitePropertyId);
            var items = userNotifications.Select(item => new
            {
                Title = item.UserNotification.Title,
                Body = item.UserNotification.Body,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "UserNotifications.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(AhlanFeekumPermissions.UserNotifications.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> usernotificationIds)
        {
            await _userNotificationRepository.DeleteManyAsync(usernotificationIds);
        }

        [Authorize(AhlanFeekumPermissions.UserNotifications.Delete)]
        public virtual async Task DeleteAllAsync(GetUserNotificationsInput input)
        {
            await _userNotificationRepository.DeleteAllAsync(input.FilterText, input.Title, input.Body, input.UserProfileId, input.SitePropertyId);
        }
        public virtual async Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new UserNotificationDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new AhlanFeekum.Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}