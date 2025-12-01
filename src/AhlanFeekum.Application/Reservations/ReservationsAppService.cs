using AhlanFeekum.Reservations;
using AhlanFeekum.Shared;
using AhlanFeekum.Permissions;
using AhlanFeekum.Reservations;
using AhlanFeekum.Shared;
using AhlanFeekum.Shared;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.UserProfiles;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;

namespace AhlanFeekum.Reservations
{

    [Authorize(AhlanFeekumPermissions.Reservations.Default)]
    public abstract class ReservationsAppServiceBase : AhlanFeekumAppService
    {
        protected IDistributedCache<ReservationDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IReservationRepository _reservationRepository;
        protected ReservationManager _reservationManager;

        protected IRepository<AhlanFeekum.UserProfiles.UserProfile, Guid> _userProfileRepository;
        protected IRepository<AhlanFeekum.SiteProperties.SiteProperty, Guid> _sitePropertyRepository;

        public ReservationsAppServiceBase(IReservationRepository reservationRepository, ReservationManager reservationManager, IDistributedCache<ReservationDownloadTokenCacheItem, string> downloadTokenCache, IRepository<AhlanFeekum.UserProfiles.UserProfile, Guid> userProfileRepository, IRepository<AhlanFeekum.SiteProperties.SiteProperty, Guid> sitePropertyRepository)
        {
            _downloadTokenCache = downloadTokenCache;
            _reservationRepository = reservationRepository;
            _reservationManager = reservationManager; _userProfileRepository = userProfileRepository;
            _sitePropertyRepository = sitePropertyRepository;

        }

        public virtual async Task<PagedResultDto<ReservationWithNavigationPropertiesDto>> GetListAsync(GetReservationsInput input)
        {
            var totalCount = await _reservationRepository.GetCountAsync(input.FilterText, input.FromeDateMin, input.FromeDateMax, input.ToDateMin, input.ToDateMax, input.CheckInDateMin, input.CheckInDateMax, input.CheckOutDateMin, input.CheckOutDateMax, input.NumberOfGuestMin, input.NumberOfGuestMax, input.PriceMin, input.PriceMax, input.DiscountMin, input.DiscountMax, input.ReservationStatus, input.Notes, input.ReservationPaymentMethod, input.IsPaid, input.Description, input.UserProfileId, input.SitePropertyId);
            var items = await _reservationRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.FromeDateMin, input.FromeDateMax, input.ToDateMin, input.ToDateMax, input.CheckInDateMin, input.CheckInDateMax, input.CheckOutDateMin, input.CheckOutDateMax, input.NumberOfGuestMin, input.NumberOfGuestMax, input.PriceMin, input.PriceMax, input.DiscountMin, input.DiscountMax, input.ReservationStatus, input.Notes, input.ReservationPaymentMethod, input.IsPaid, input.Description, input.UserProfileId, input.SitePropertyId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ReservationWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ReservationWithNavigationProperties>, List<ReservationWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<ReservationWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<ReservationWithNavigationProperties, ReservationWithNavigationPropertiesDto>
                (await _reservationRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<ReservationDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Reservation, ReservationDto>(await _reservationRepository.GetAsync(id));
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

        [Authorize(AhlanFeekumPermissions.Reservations.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _reservationRepository.DeleteAsync(id);
        }

        [Authorize(AhlanFeekumPermissions.Reservations.Create)]
        public virtual async Task<ReservationDto> CreateAsync(ReservationCreateDto input)
        {
            if (input.UserProfileId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["UserProfile"]]);
            }
            if (input.SitePropertyId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["SiteProperty"]]);
            }

            var reservation = await _reservationManager.CreateAsync(
            input.UserProfileId, input.SitePropertyId, input.FromeDate, input.ToDate, input.Price, input.ReservationStatus, input.IsPaid, input.CheckInDate, input.CheckOutDate, input.NumberOfGuest, input.Discount, input.Notes, input.ReservationPaymentMethod, input.Description
            );

            return ObjectMapper.Map<Reservation, ReservationDto>(reservation);
        }

        [Authorize(AhlanFeekumPermissions.Reservations.Edit)]
        public virtual async Task<ReservationDto> UpdateAsync(Guid id, ReservationUpdateDto input)
        {
            if (input.UserProfileId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["UserProfile"]]);
            }
            if (input.SitePropertyId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["SiteProperty"]]);
            }

            var reservation = await _reservationManager.UpdateAsync(
            id,
            input.UserProfileId, input.SitePropertyId, input.FromeDate, input.ToDate, input.Price, input.ReservationStatus, input.IsPaid, input.CheckInDate, input.CheckOutDate, input.NumberOfGuest, input.Discount, input.Notes, input.ReservationPaymentMethod, input.Description, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Reservation, ReservationDto>(reservation);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ReservationExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var reservations = await _reservationRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.FromeDateMin, input.FromeDateMax, input.ToDateMin, input.ToDateMax, input.CheckInDateMin, input.CheckInDateMax, input.CheckOutDateMin, input.CheckOutDateMax, input.NumberOfGuestMin, input.NumberOfGuestMax, input.PriceMin, input.PriceMax, input.DiscountMin, input.DiscountMax, input.ReservationStatus, input.Notes, input.ReservationPaymentMethod, input.IsPaid, input.Description, input.UserProfileId, input.SitePropertyId);
            var items = reservations.Select(item => new
            {
                FromeDate = item.Reservation.FromeDate,
                ToDate = item.Reservation.ToDate,
                CheckInDate = item.Reservation.CheckInDate,
                CheckOutDate = item.Reservation.CheckOutDate,
                NumberOfGuest = item.Reservation.NumberOfGuest,
                Price = item.Reservation.Price,
                Discount = item.Reservation.Discount,
                ReservationStatus = item.Reservation.ReservationStatus,
                Notes = item.Reservation.Notes,
                ReservationPaymentMethod = item.Reservation.ReservationPaymentMethod,
                IsPaid = item.Reservation.IsPaid,
                Description = item.Reservation.Description,

                UserProfile = item.UserProfile?.Name,
                SiteProperty = item.SiteProperty?.PropertyTitle,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Reservations.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(AhlanFeekumPermissions.Reservations.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> reservationIds)
        {
            await _reservationRepository.DeleteManyAsync(reservationIds);
        }

        [Authorize(AhlanFeekumPermissions.Reservations.Delete)]
        public virtual async Task DeleteAllAsync(GetReservationsInput input)
        {
            await _reservationRepository.DeleteAllAsync(input.FilterText, input.FromeDateMin, input.FromeDateMax, input.ToDateMin, input.ToDateMax, input.CheckInDateMin, input.CheckInDateMax, input.CheckOutDateMin, input.CheckOutDateMax, input.NumberOfGuestMin, input.NumberOfGuestMax, input.PriceMin, input.PriceMax, input.DiscountMin, input.DiscountMax, input.ReservationStatus, input.Notes, input.ReservationPaymentMethod, input.IsPaid, input.Description, input.UserProfileId, input.SitePropertyId);
        }
        public virtual async Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new ReservationDownloadTokenCacheItem { Token = token },
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