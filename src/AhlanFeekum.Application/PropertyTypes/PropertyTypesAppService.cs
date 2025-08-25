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
using AhlanFeekum.PropertyTypes;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using AhlanFeekum.Shared;

namespace AhlanFeekum.PropertyTypes
{

    [Authorize(AhlanFeekumPermissions.PropertyTypes.Default)]
    public abstract class PropertyTypesAppServiceBase : AhlanFeekumAppService
    {
        protected IDistributedCache<PropertyTypeDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IPropertyTypeRepository _propertyTypeRepository;
        protected PropertyTypeManager _propertyTypeManager;

        public PropertyTypesAppServiceBase(IPropertyTypeRepository propertyTypeRepository, PropertyTypeManager propertyTypeManager, IDistributedCache<PropertyTypeDownloadTokenCacheItem, string> downloadTokenCache)
        {
            _downloadTokenCache = downloadTokenCache;
            _propertyTypeRepository = propertyTypeRepository;
            _propertyTypeManager = propertyTypeManager;

        }

        public virtual async Task<PagedResultDto<PropertyTypeDto>> GetListAsync(GetPropertyTypesInput input)
        {
            var totalCount = await _propertyTypeRepository.GetCountAsync(input.FilterText, input.Title, input.OrderMin, input.OrderMax, input.IsActive);
            var items = await _propertyTypeRepository.GetListAsync(input.FilterText, input.Title, input.OrderMin, input.OrderMax, input.IsActive, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<PropertyTypeDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<PropertyType>, List<PropertyTypeDto>>(items)
            };
        }

        public virtual async Task<PropertyTypeDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<PropertyType, PropertyTypeDto>(await _propertyTypeRepository.GetAsync(id));
        }

        [Authorize(AhlanFeekumPermissions.PropertyTypes.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _propertyTypeRepository.DeleteAsync(id);
        }

        [Authorize(AhlanFeekumPermissions.PropertyTypes.Create)]
        public virtual async Task<PropertyTypeDto> CreateAsync(PropertyTypeCreateDto input)
        {

            var propertyType = await _propertyTypeManager.CreateAsync(
            input.Title, input.Order, input.IsActive
            );

            return ObjectMapper.Map<PropertyType, PropertyTypeDto>(propertyType);
        }

        [Authorize(AhlanFeekumPermissions.PropertyTypes.Edit)]
        public virtual async Task<PropertyTypeDto> UpdateAsync(Guid id, PropertyTypeUpdateDto input)
        {

            var propertyType = await _propertyTypeManager.UpdateAsync(
            id,
            input.Title, input.Order, input.IsActive, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<PropertyType, PropertyTypeDto>(propertyType);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(PropertyTypeExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _propertyTypeRepository.GetListAsync(input.FilterText, input.Title, input.OrderMin, input.OrderMax, input.IsActive);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<PropertyType>, List<PropertyTypeExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "PropertyTypes.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(AhlanFeekumPermissions.PropertyTypes.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> propertytypeIds)
        {
            await _propertyTypeRepository.DeleteManyAsync(propertytypeIds);
        }

        [Authorize(AhlanFeekumPermissions.PropertyTypes.Delete)]
        public virtual async Task DeleteAllAsync(GetPropertyTypesInput input)
        {
            await _propertyTypeRepository.DeleteAllAsync(input.FilterText, input.Title, input.OrderMin, input.OrderMax, input.IsActive);
        }
        public virtual async Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new PropertyTypeDownloadTokenCacheItem { Token = token },
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