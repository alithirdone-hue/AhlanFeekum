using AhlanFeekum.Shared;
using AhlanFeekum.UserPayments;
using AhlanFeekum.Permissions;
using AhlanFeekum.Reservations;
using AhlanFeekum.Shared;
using AhlanFeekum.Shared;
using AhlanFeekum.UserPayments;
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

namespace AhlanFeekum.UserPayments
{

    [Authorize(AhlanFeekumPermissions.UserPayments.Default)]
    public abstract class UserPaymentsAppServiceBase : AhlanFeekumAppService
    {
        protected IDistributedCache<UserPaymentDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IUserPaymentRepository _userPaymentRepository;
        protected UserPaymentManager _userPaymentManager;

        protected IRepository<AhlanFeekum.UserProfiles.UserProfile, Guid> _userProfileRepository;
        protected IRepository<AhlanFeekum.Reservations.Reservation, Guid> _reservationRepository;

        public UserPaymentsAppServiceBase(IUserPaymentRepository userPaymentRepository, UserPaymentManager userPaymentManager, IDistributedCache<UserPaymentDownloadTokenCacheItem, string> downloadTokenCache, IRepository<AhlanFeekum.UserProfiles.UserProfile, Guid> userProfileRepository, IRepository<AhlanFeekum.Reservations.Reservation, Guid> reservationRepository)
        {
            _downloadTokenCache = downloadTokenCache;
            _userPaymentRepository = userPaymentRepository;
            _userPaymentManager = userPaymentManager; _userProfileRepository = userProfileRepository;
            _reservationRepository = reservationRepository;

        }

        public virtual async Task<PagedResultDto<UserPaymentWithNavigationPropertiesDto>> GetListAsync(GetUserPaymentsInput input)
        {
            var totalCount = await _userPaymentRepository.GetCountAsync(input.FilterText, input.AmountMin, input.AmountMax, input.Currency, input.Description, input.ReceiptEmail, input.AmountCapturableMin, input.AmountCapturableMax, input.AmountReceivedMin, input.AmountReceivedMax, input.ConfirmationMethod, input.Status, input.StripPaymentId, input.StripClientSecret, input.CreatedMin, input.CreatedMax, input.PaymentMethod, input.UserProfileId, input.ReservationId);
            var items = await _userPaymentRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.AmountMin, input.AmountMax, input.Currency, input.Description, input.ReceiptEmail, input.AmountCapturableMin, input.AmountCapturableMax, input.AmountReceivedMin, input.AmountReceivedMax, input.ConfirmationMethod, input.Status, input.StripPaymentId, input.StripClientSecret, input.CreatedMin, input.CreatedMax, input.PaymentMethod, input.UserProfileId, input.ReservationId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<UserPaymentWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<UserPaymentWithNavigationProperties>, List<UserPaymentWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<UserPaymentWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<UserPaymentWithNavigationProperties, UserPaymentWithNavigationPropertiesDto>
                (await _userPaymentRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<UserPaymentDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<UserPayment, UserPaymentDto>(await _userPaymentRepository.GetAsync(id));
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
                    x => x.Notes != null &&
                         x.Notes.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<AhlanFeekum.Reservations.Reservation>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<AhlanFeekum.Reservations.Reservation>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        [Authorize(AhlanFeekumPermissions.UserPayments.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _userPaymentRepository.DeleteAsync(id);
        }

        [Authorize(AhlanFeekumPermissions.UserPayments.Create)]
        public virtual async Task<UserPaymentDto> CreateAsync(UserPaymentCreateDto input)
        {
            if (input.UserProfileId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["UserProfile"]]);
            }
            if (input.ReservationId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Reservation"]]);
            }

            var userPayment = await _userPaymentManager.CreateAsync(
            input.UserProfileId, input.ReservationId, input.Amount, input.AmountCapturable, input.AmountReceived, input.Status, input.Created, input.PaymentMethod, input.Currency, input.Description, input.ReceiptEmail, input.ConfirmationMethod, input.StripPaymentId, input.StripClientSecret
            );

            return ObjectMapper.Map<UserPayment, UserPaymentDto>(userPayment);
        }

        [Authorize(AhlanFeekumPermissions.UserPayments.Edit)]
        public virtual async Task<UserPaymentDto> UpdateAsync(Guid id, UserPaymentUpdateDto input)
        {
            if (input.UserProfileId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["UserProfile"]]);
            }
            if (input.ReservationId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Reservation"]]);
            }

            var userPayment = await _userPaymentManager.UpdateAsync(
            id,
            input.UserProfileId, input.ReservationId, input.Amount, input.AmountCapturable, input.AmountReceived, input.Status, input.Created, input.PaymentMethod, input.Currency, input.Description, input.ReceiptEmail, input.ConfirmationMethod, input.StripPaymentId, input.StripClientSecret, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<UserPayment, UserPaymentDto>(userPayment);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(UserPaymentExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var userPayments = await _userPaymentRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.AmountMin, input.AmountMax, input.Currency, input.Description, input.ReceiptEmail, input.AmountCapturableMin, input.AmountCapturableMax, input.AmountReceivedMin, input.AmountReceivedMax, input.ConfirmationMethod, input.Status, input.StripPaymentId, input.StripClientSecret, input.CreatedMin, input.CreatedMax, input.PaymentMethod, input.UserProfileId, input.ReservationId);
            var items = userPayments.Select(item => new
            {
                Amount = item.UserPayment.Amount,
                Currency = item.UserPayment.Currency,
                Description = item.UserPayment.Description,
                ReceiptEmail = item.UserPayment.ReceiptEmail,
                AmountCapturable = item.UserPayment.AmountCapturable,
                AmountReceived = item.UserPayment.AmountReceived,
                ConfirmationMethod = item.UserPayment.ConfirmationMethod,
                Status = item.UserPayment.Status,
                StripPaymentId = item.UserPayment.StripPaymentId,
                StripClientSecret = item.UserPayment.StripClientSecret,
                Created = item.UserPayment.Created,
                PaymentMethod = item.UserPayment.PaymentMethod,

                UserProfile = item.UserProfile?.Name,
                Reservation = item.Reservation?.Notes,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "UserPayments.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(AhlanFeekumPermissions.UserPayments.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> userpaymentIds)
        {
            await _userPaymentRepository.DeleteManyAsync(userpaymentIds);
        }

        [Authorize(AhlanFeekumPermissions.UserPayments.Delete)]
        public virtual async Task DeleteAllAsync(GetUserPaymentsInput input)
        {
            await _userPaymentRepository.DeleteAllAsync(input.FilterText, input.AmountMin, input.AmountMax, input.Currency, input.Description, input.ReceiptEmail, input.AmountCapturableMin, input.AmountCapturableMax, input.AmountReceivedMin, input.AmountReceivedMax, input.ConfirmationMethod, input.Status, input.StripPaymentId, input.StripClientSecret, input.CreatedMin, input.CreatedMax, input.PaymentMethod, input.UserProfileId, input.ReservationId);
        }
        public virtual async Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new UserPaymentDownloadTokenCacheItem { Token = token },
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