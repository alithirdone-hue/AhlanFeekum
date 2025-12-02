using AhlanFeekum.Shared;
using AhlanFeekum.Reservations;
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
using AhlanFeekum.CashPayments;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using AhlanFeekum.Shared;

namespace AhlanFeekum.CashPayments
{

    [Authorize(AhlanFeekumPermissions.CashPayments.Default)]
    public abstract class CashPaymentsAppServiceBase : AhlanFeekumAppService
    {
        protected IDistributedCache<CashPaymentDownloadTokenCacheItem, string> _downloadTokenCache;
        protected ICashPaymentRepository _cashPaymentRepository;
        protected CashPaymentManager _cashPaymentManager;

        protected IRepository<AhlanFeekum.UserProfiles.UserProfile, Guid> _userProfileRepository;
        protected IRepository<AhlanFeekum.Reservations.Reservation, Guid> _reservationRepository;

        public CashPaymentsAppServiceBase(ICashPaymentRepository cashPaymentRepository, CashPaymentManager cashPaymentManager, IDistributedCache<CashPaymentDownloadTokenCacheItem, string> downloadTokenCache, IRepository<AhlanFeekum.UserProfiles.UserProfile, Guid> userProfileRepository, IRepository<AhlanFeekum.Reservations.Reservation, Guid> reservationRepository)
        {
            _downloadTokenCache = downloadTokenCache;
            _cashPaymentRepository = cashPaymentRepository;
            _cashPaymentManager = cashPaymentManager; _userProfileRepository = userProfileRepository;
            _reservationRepository = reservationRepository;

        }

        public virtual async Task<PagedResultDto<CashPaymentWithNavigationPropertiesDto>> GetListAsync(GetCashPaymentsInput input)
        {
            var totalCount = await _cashPaymentRepository.GetCountAsync(input.FilterText, input.AmountMin, input.AmountMax, input.Currency, input.PaymentDateMin, input.PaymentDateMax, input.Description, input.Status, input.UserProfileId, input.ReservationId);
            var items = await _cashPaymentRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.AmountMin, input.AmountMax, input.Currency, input.PaymentDateMin, input.PaymentDateMax, input.Description, input.Status, input.UserProfileId, input.ReservationId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<CashPaymentWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<CashPaymentWithNavigationProperties>, List<CashPaymentWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<CashPaymentWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<CashPaymentWithNavigationProperties, CashPaymentWithNavigationPropertiesDto>
                (await _cashPaymentRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<CashPaymentDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<CashPayment, CashPaymentDto>(await _cashPaymentRepository.GetAsync(id));
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

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetReservationLookupAsync(LookupRequestDto input)
        {
            var query = (await _reservationRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Description != null &&
                         x.Description.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<AhlanFeekum.Reservations.Reservation>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<AhlanFeekum.Reservations.Reservation>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        [Authorize(AhlanFeekumPermissions.CashPayments.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _cashPaymentRepository.DeleteAsync(id);
        }

        [Authorize(AhlanFeekumPermissions.CashPayments.Create)]
        public virtual async Task<CashPaymentDto> CreateAsync(CashPaymentCreateDto input)
        {
            if (input.UserProfileId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["UserProfile"]]);
            }
            if (input.ReservationId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Reservation"]]);
            }

            var cashPayment = await _cashPaymentManager.CreateAsync(
            input.UserProfileId, input.ReservationId, input.Amount, input.Currency, input.PaymentDate, input.Status, input.Description
            );

            return ObjectMapper.Map<CashPayment, CashPaymentDto>(cashPayment);
        }

        [Authorize(AhlanFeekumPermissions.CashPayments.Edit)]
        public virtual async Task<CashPaymentDto> UpdateAsync(Guid id, CashPaymentUpdateDto input)
        {
            if (input.UserProfileId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["UserProfile"]]);
            }
            if (input.ReservationId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Reservation"]]);
            }

            var cashPayment = await _cashPaymentManager.UpdateAsync(
            id,
            input.UserProfileId, input.ReservationId, input.Amount, input.Currency, input.PaymentDate, input.Status, input.Description, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<CashPayment, CashPaymentDto>(cashPayment);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(CashPaymentExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var cashPayments = await _cashPaymentRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.AmountMin, input.AmountMax, input.Currency, input.PaymentDateMin, input.PaymentDateMax, input.Description, input.Status, input.UserProfileId, input.ReservationId);
            var items = cashPayments.Select(item => new
            {
                Amount = item.CashPayment.Amount,
                Currency = item.CashPayment.Currency,
                PaymentDate = item.CashPayment.PaymentDate,
                Description = item.CashPayment.Description,
                Status = item.CashPayment.Status,

                UserProfile = item.UserProfile?.Name,
                Reservation = item.Reservation?.Description,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "CashPayments.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(AhlanFeekumPermissions.CashPayments.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> cashpaymentIds)
        {
            await _cashPaymentRepository.DeleteManyAsync(cashpaymentIds);
        }

        [Authorize(AhlanFeekumPermissions.CashPayments.Delete)]
        public virtual async Task DeleteAllAsync(GetCashPaymentsInput input)
        {
            await _cashPaymentRepository.DeleteAllAsync(input.FilterText, input.AmountMin, input.AmountMax, input.Currency, input.PaymentDateMin, input.PaymentDateMax, input.Description, input.Status, input.UserProfileId, input.ReservationId);
        }
        public virtual async Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new CashPaymentDownloadTokenCacheItem { Token = token },
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