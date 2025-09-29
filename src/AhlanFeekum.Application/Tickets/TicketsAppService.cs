using AhlanFeekum.Shared;
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
using AhlanFeekum.Tickets;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using AhlanFeekum.Shared;

namespace AhlanFeekum.Tickets
{

    [Authorize(AhlanFeekumPermissions.Tickets.Default)]
    public abstract class TicketsAppServiceBase : AhlanFeekumAppService
    {
        protected IDistributedCache<TicketDownloadTokenCacheItem, string> _downloadTokenCache;
        protected ITicketRepository _ticketRepository;
        protected TicketManager _ticketManager;

        protected IRepository<AhlanFeekum.UserProfiles.UserProfile, Guid> _userProfileRepository;

        public TicketsAppServiceBase(ITicketRepository ticketRepository, TicketManager ticketManager, IDistributedCache<TicketDownloadTokenCacheItem, string> downloadTokenCache, IRepository<AhlanFeekum.UserProfiles.UserProfile, Guid> userProfileRepository)
        {
            _downloadTokenCache = downloadTokenCache;
            _ticketRepository = ticketRepository;
            _ticketManager = ticketManager; _userProfileRepository = userProfileRepository;

        }

        public virtual async Task<PagedResultDto<TicketWithNavigationPropertiesDto>> GetListAsync(GetTicketsInput input)
        {
            var totalCount = await _ticketRepository.GetCountAsync(input.FilterText, input.FirstName, input.LastName, input.Description, input.IsFixed, input.UserProfileId);
            var items = await _ticketRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.FirstName, input.LastName, input.Description, input.IsFixed, input.UserProfileId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<TicketWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<TicketWithNavigationProperties>, List<TicketWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<TicketWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<TicketWithNavigationProperties, TicketWithNavigationPropertiesDto>
                (await _ticketRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<TicketDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Ticket, TicketDto>(await _ticketRepository.GetAsync(id));
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

        [Authorize(AhlanFeekumPermissions.Tickets.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _ticketRepository.DeleteAsync(id);
        }

        [Authorize(AhlanFeekumPermissions.Tickets.Create)]
        public virtual async Task<TicketDto> CreateAsync(TicketCreateDto input)
        {

            var ticket = await _ticketManager.CreateAsync(
            input.UserProfileId, input.FirstName, input.LastName, input.Description, input.IsFixed
            );

            return ObjectMapper.Map<Ticket, TicketDto>(ticket);
        }

        [Authorize(AhlanFeekumPermissions.Tickets.Edit)]
        public virtual async Task<TicketDto> UpdateAsync(Guid id, TicketUpdateDto input)
        {

            var ticket = await _ticketManager.UpdateAsync(
            id,
            input.UserProfileId, input.FirstName, input.LastName, input.Description, input.IsFixed, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Ticket, TicketDto>(ticket);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(TicketExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var tickets = await _ticketRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.FirstName, input.LastName, input.Description, input.IsFixed, input.UserProfileId);
            var items = tickets.Select(item => new
            {
                FirstName = item.Ticket.FirstName,
                LastName = item.Ticket.LastName,
                Description = item.Ticket.Description,
                IsFixed = item.Ticket.IsFixed,

                UserProfile = item.UserProfile?.Name,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Tickets.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(AhlanFeekumPermissions.Tickets.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> ticketIds)
        {
            await _ticketRepository.DeleteManyAsync(ticketIds);
        }

        [Authorize(AhlanFeekumPermissions.Tickets.Delete)]
        public virtual async Task DeleteAllAsync(GetTicketsInput input)
        {
            await _ticketRepository.DeleteAllAsync(input.FilterText, input.FirstName, input.LastName, input.Description, input.IsFixed, input.UserProfileId);
        }
        public virtual async Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new TicketDownloadTokenCacheItem { Token = token },
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