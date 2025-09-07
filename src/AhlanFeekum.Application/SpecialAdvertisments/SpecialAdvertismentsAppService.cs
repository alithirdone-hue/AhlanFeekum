using AhlanFeekum.Shared;
using AhlanFeekum.SiteProperties;
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
using AhlanFeekum.SpecialAdvertisments;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using AhlanFeekum.Shared;

namespace AhlanFeekum.SpecialAdvertisments
{

    [Authorize(AhlanFeekumPermissions.SpecialAdvertisments.Default)]
    public abstract class SpecialAdvertismentsAppServiceBase : AhlanFeekumAppService
    {
        protected IDistributedCache<SpecialAdvertismentDownloadTokenCacheItem, string> _downloadTokenCache;
        protected ISpecialAdvertismentRepository _specialAdvertismentRepository;
        protected SpecialAdvertismentManager _specialAdvertismentManager;

        protected IRepository<AhlanFeekum.SiteProperties.SiteProperty, Guid> _sitePropertyRepository;

        public SpecialAdvertismentsAppServiceBase(ISpecialAdvertismentRepository specialAdvertismentRepository, SpecialAdvertismentManager specialAdvertismentManager, IDistributedCache<SpecialAdvertismentDownloadTokenCacheItem, string> downloadTokenCache, IRepository<AhlanFeekum.SiteProperties.SiteProperty, Guid> sitePropertyRepository)
        {
            _downloadTokenCache = downloadTokenCache;
            _specialAdvertismentRepository = specialAdvertismentRepository;
            _specialAdvertismentManager = specialAdvertismentManager; _sitePropertyRepository = sitePropertyRepository;

        }

        public virtual async Task<PagedResultDto<SpecialAdvertismentWithNavigationPropertiesDto>> GetListAsync(GetSpecialAdvertismentsInput input)
        {
            var totalCount = await _specialAdvertismentRepository.GetCountAsync(input.FilterText, input.Image, input.OrderMin, input.OrderMax, input.IsActive, input.SitePropertyId);
            var items = await _specialAdvertismentRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Image, input.OrderMin, input.OrderMax, input.IsActive, input.SitePropertyId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<SpecialAdvertismentWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<SpecialAdvertismentWithNavigationProperties>, List<SpecialAdvertismentWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<SpecialAdvertismentWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<SpecialAdvertismentWithNavigationProperties, SpecialAdvertismentWithNavigationPropertiesDto>
                (await _specialAdvertismentRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<SpecialAdvertismentDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<SpecialAdvertisment, SpecialAdvertismentDto>(await _specialAdvertismentRepository.GetAsync(id));
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

        [Authorize(AhlanFeekumPermissions.SpecialAdvertisments.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _specialAdvertismentRepository.DeleteAsync(id);
        }

        [Authorize(AhlanFeekumPermissions.SpecialAdvertisments.Create)]
        public virtual async Task<SpecialAdvertismentDto> CreateAsync(SpecialAdvertismentCreateDto input)
        {
            if (input.SitePropertyId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["SiteProperty"]]);
            }

            var specialAdvertisment = await _specialAdvertismentManager.CreateAsync(
            input.SitePropertyId, input.Image, input.Order, input.IsActive
            );

            return ObjectMapper.Map<SpecialAdvertisment, SpecialAdvertismentDto>(specialAdvertisment);
        }

        [Authorize(AhlanFeekumPermissions.SpecialAdvertisments.Edit)]
        public virtual async Task<SpecialAdvertismentDto> UpdateAsync(Guid id, SpecialAdvertismentUpdateDto input)
        {
            if (input.SitePropertyId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["SiteProperty"]]);
            }

            var specialAdvertisment = await _specialAdvertismentManager.UpdateAsync(
            id,
            input.SitePropertyId, input.Image, input.Order, input.IsActive, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<SpecialAdvertisment, SpecialAdvertismentDto>(specialAdvertisment);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(SpecialAdvertismentExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var specialAdvertisments = await _specialAdvertismentRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Image, input.OrderMin, input.OrderMax, input.IsActive, input.SitePropertyId);
            var items = specialAdvertisments.Select(item => new
            {
                Image = item.SpecialAdvertisment.Image,
                Order = item.SpecialAdvertisment.Order,
                IsActive = item.SpecialAdvertisment.IsActive,

                SiteProperty = item.SiteProperty?.PropertyTitle,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "SpecialAdvertisments.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(AhlanFeekumPermissions.SpecialAdvertisments.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> specialadvertismentIds)
        {
            await _specialAdvertismentRepository.DeleteManyAsync(specialadvertismentIds);
        }

        [Authorize(AhlanFeekumPermissions.SpecialAdvertisments.Delete)]
        public virtual async Task DeleteAllAsync(GetSpecialAdvertismentsInput input)
        {
            await _specialAdvertismentRepository.DeleteAllAsync(input.FilterText, input.Image, input.OrderMin, input.OrderMax, input.IsActive, input.SitePropertyId);
        }
        public virtual async Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new SpecialAdvertismentDownloadTokenCacheItem { Token = token },
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