using AhlanFeekum.SiteProperties;
using AhlanFeekum.UserProfiles;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using AhlanFeekum.EntityFrameworkCore;

namespace AhlanFeekum.UserNotifications
{
    public abstract class EfCoreUserNotificationRepositoryBase : EfCoreRepository<AhlanFeekumDbContext, UserNotification, Guid>
    {
        public EfCoreUserNotificationRepositoryBase(IDbContextProvider<AhlanFeekumDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        string? title = null,
            string? body = null,
            Guid? userProfileId = null,
            Guid? sitePropertyId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();

            query = ApplyFilter(query, filterText, title, body, userProfileId, sitePropertyId);

            var ids = query.Select(x => x.UserNotification.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<UserNotificationWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id).Include(x => x.UserProfiles).Include(x => x.SiteProperties)
                .Select(userNotification => new UserNotificationWithNavigationProperties
                {
                    UserNotification = userNotification,
                    UserProfiles = (from userNotificationUserProfiles in userNotification.UserProfiles
                                    join _userProfile in dbContext.Set<UserProfile>() on userNotificationUserProfiles.UserProfileId equals _userProfile.Id
                                    select _userProfile).ToList(),
                    SiteProperties = (from userNotificationSiteProperties in userNotification.SiteProperties
                                      join _siteProperty in dbContext.Set<SiteProperty>() on userNotificationSiteProperties.SitePropertyId equals _siteProperty.Id
                                      select _siteProperty).ToList()
                }).FirstOrDefault();
        }

        public virtual async Task<List<UserNotificationWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? title = null,
            string? body = null,
            Guid? userProfileId = null,
            Guid? sitePropertyId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, title, body, userProfileId, sitePropertyId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? UserNotificationConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<UserNotificationWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from userNotification in (await GetDbSetAsync())

                   select new UserNotificationWithNavigationProperties
                   {
                       UserNotification = userNotification,
                       UserProfiles = new List<UserProfile>(),
                       SiteProperties = new List<SiteProperty>()
                   };
        }

        protected virtual IQueryable<UserNotificationWithNavigationProperties> ApplyFilter(
            IQueryable<UserNotificationWithNavigationProperties> query,
            string? filterText,
            string? title = null,
            string? body = null,
            Guid? userProfileId = null,
            Guid? sitePropertyId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.UserNotification.Title!.Contains(filterText!) || e.UserNotification.Body!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(title), e => e.UserNotification.Title.Contains(title))
                    .WhereIf(!string.IsNullOrWhiteSpace(body), e => e.UserNotification.Body.Contains(body))
                    .WhereIf(userProfileId != null && userProfileId != Guid.Empty, e => e.UserNotification.UserProfiles.Any(x => x.UserProfileId == userProfileId))
                    .WhereIf(sitePropertyId != null && sitePropertyId != Guid.Empty, e => e.UserNotification.SiteProperties.Any(x => x.SitePropertyId == sitePropertyId));
        }

        public virtual async Task<List<UserNotification>> GetListAsync(
            string? filterText = null,
            string? title = null,
            string? body = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, title, body);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? UserNotificationConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? title = null,
            string? body = null,
            Guid? userProfileId = null,
            Guid? sitePropertyId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, title, body, userProfileId, sitePropertyId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<UserNotification> ApplyFilter(
            IQueryable<UserNotification> query,
            string? filterText = null,
            string? title = null,
            string? body = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Title!.Contains(filterText!) || e.Body!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(title), e => e.Title.Contains(title))
                    .WhereIf(!string.IsNullOrWhiteSpace(body), e => e.Body.Contains(body));
        }
    }
}