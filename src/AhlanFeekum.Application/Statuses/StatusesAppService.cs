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
using AhlanFeekum.Statuses;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using AhlanFeekum.Shared;

namespace AhlanFeekum.Statuses
{

    [Authorize(AhlanFeekumPermissions.Statuses.Default)]
    public abstract class StatusesAppServiceBase : AhlanFeekumAppService
    {
        protected IDistributedCache<StatusDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IStatusRepository _statusRepository;
        protected StatusManager _statusManager;

        public StatusesAppServiceBase(IStatusRepository statusRepository, StatusManager statusManager, IDistributedCache<StatusDownloadTokenCacheItem, string> downloadTokenCache)
        {
            _downloadTokenCache = downloadTokenCache;
            _statusRepository = statusRepository;
            _statusManager = statusManager;

        }

        public virtual async Task<PagedResultDto<StatusDto>> GetListAsync(GetStatusesInput input)
        {
            var totalCount = await _statusRepository.GetCountAsync(input.FilterText, input.Name, input.OrderMin, input.OrderMax, input.IsActive);
            var items = await _statusRepository.GetListAsync(input.FilterText, input.Name, input.OrderMin, input.OrderMax, input.IsActive, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<StatusDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Status>, List<StatusDto>>(items)
            };
        }

        public virtual async Task<StatusDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Status, StatusDto>(await _statusRepository.GetAsync(id));
        }

        [Authorize(AhlanFeekumPermissions.Statuses.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _statusRepository.DeleteAsync(id);
        }

        [Authorize(AhlanFeekumPermissions.Statuses.Create)]
        public virtual async Task<StatusDto> CreateAsync(StatusCreateDto input)
        {

            var status = await _statusManager.CreateAsync(
            input.Name, input.Order, input.IsActive
            );

            return ObjectMapper.Map<Status, StatusDto>(status);
        }

        [Authorize(AhlanFeekumPermissions.Statuses.Edit)]
        public virtual async Task<StatusDto> UpdateAsync(Guid id, StatusUpdateDto input)
        {

            var status = await _statusManager.UpdateAsync(
            id,
            input.Name, input.Order, input.IsActive, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Status, StatusDto>(status);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(StatusExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _statusRepository.GetListAsync(input.FilterText, input.Name, input.OrderMin, input.OrderMax, input.IsActive);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Status>, List<StatusExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Statuses.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(AhlanFeekumPermissions.Statuses.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> statusIds)
        {
            await _statusRepository.DeleteManyAsync(statusIds);
        }

        [Authorize(AhlanFeekumPermissions.Statuses.Delete)]
        public virtual async Task DeleteAllAsync(GetStatusesInput input)
        {
            await _statusRepository.DeleteAllAsync(input.FilterText, input.Name, input.OrderMin, input.OrderMax, input.IsActive);
        }
        public virtual async Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new StatusDownloadTokenCacheItem { Token = token },
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