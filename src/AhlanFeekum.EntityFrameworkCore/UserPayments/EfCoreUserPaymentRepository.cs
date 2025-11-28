using AhlanFeekum.UserPayments;
using AhlanFeekum.Reservations;
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

namespace AhlanFeekum.UserPayments
{
    public abstract class EfCoreUserPaymentRepositoryBase : EfCoreRepository<AhlanFeekumDbContext, UserPayment, Guid>
    {
        public EfCoreUserPaymentRepositoryBase(IDbContextProvider<AhlanFeekumDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            string? description = null,
            string? receiptEmail = null,
            long? amountCapturableMin = null,
            long? amountCapturableMax = null,
            long? amountReceivedMin = null,
            long? amountReceivedMax = null,
            string? confirmationMethod = null,
            UserPaymentStatus? status = null,
            string? stripPaymentId = null,
            string? stripClientSecret = null,
            string? created = null,
            Guid? userProfileId = null,
            Guid? reservationId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();

            query = ApplyFilter(query, filterText, amountMin, amountMax, currency, description, receiptEmail, amountCapturableMin, amountCapturableMax, amountReceivedMin, amountReceivedMax, confirmationMethod, status, stripPaymentId, stripClientSecret, created, userProfileId, reservationId);

            var ids = query.Select(x => x.UserPayment.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<UserPaymentWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(userPayment => new UserPaymentWithNavigationProperties
                {
                    UserPayment = userPayment,
                    UserProfile = dbContext.Set<UserProfile>().FirstOrDefault(c => c.Id == userPayment.UserProfileId),
                    Reservation = dbContext.Set<Reservation>().FirstOrDefault(c => c.Id == userPayment.ReservationId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<UserPaymentWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            string? description = null,
            string? receiptEmail = null,
            long? amountCapturableMin = null,
            long? amountCapturableMax = null,
            long? amountReceivedMin = null,
            long? amountReceivedMax = null,
            string? confirmationMethod = null,
            UserPaymentStatus? status = null,
            string? stripPaymentId = null,
            string? stripClientSecret = null,
            string? created = null,
            Guid? userProfileId = null,
            Guid? reservationId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, amountMin, amountMax, currency, description, receiptEmail, amountCapturableMin, amountCapturableMax, amountReceivedMin, amountReceivedMax, confirmationMethod, status, stripPaymentId, stripClientSecret, created, userProfileId, reservationId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? UserPaymentConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<UserPaymentWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from userPayment in (await GetDbSetAsync())
                   join userProfile in (await GetDbContextAsync()).Set<UserProfile>() on userPayment.UserProfileId equals userProfile.Id into userProfiles
                   from userProfile in userProfiles.DefaultIfEmpty()
                   join reservation in (await GetDbContextAsync()).Set<Reservation>() on userPayment.ReservationId equals reservation.Id into reservations
                   from reservation in reservations.DefaultIfEmpty()
                   select new UserPaymentWithNavigationProperties
                   {
                       UserPayment = userPayment,
                       UserProfile = userProfile,
                       Reservation = reservation
                   };
        }

        protected virtual IQueryable<UserPaymentWithNavigationProperties> ApplyFilter(
            IQueryable<UserPaymentWithNavigationProperties> query,
            string? filterText,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            string? description = null,
            string? receiptEmail = null,
            long? amountCapturableMin = null,
            long? amountCapturableMax = null,
            long? amountReceivedMin = null,
            long? amountReceivedMax = null,
            string? confirmationMethod = null,
            UserPaymentStatus? status = null,
            string? stripPaymentId = null,
            string? stripClientSecret = null,
            string? created = null,
            Guid? userProfileId = null,
            Guid? reservationId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.UserPayment.Currency!.Contains(filterText!) || e.UserPayment.Description!.Contains(filterText!) || e.UserPayment.ReceiptEmail!.Contains(filterText!) || e.UserPayment.ConfirmationMethod!.Contains(filterText!) || e.UserPayment.StripPaymentId!.Contains(filterText!) || e.UserPayment.StripClientSecret!.Contains(filterText!) || e.UserPayment.Created!.Contains(filterText!))
                    .WhereIf(amountMin.HasValue, e => e.UserPayment.Amount >= amountMin!.Value)
                    .WhereIf(amountMax.HasValue, e => e.UserPayment.Amount <= amountMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(currency), e => e.UserPayment.Currency.Contains(currency))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.UserPayment.Description.Contains(description))
                    .WhereIf(!string.IsNullOrWhiteSpace(receiptEmail), e => e.UserPayment.ReceiptEmail.Contains(receiptEmail))
                    .WhereIf(amountCapturableMin.HasValue, e => e.UserPayment.AmountCapturable >= amountCapturableMin!.Value)
                    .WhereIf(amountCapturableMax.HasValue, e => e.UserPayment.AmountCapturable <= amountCapturableMax!.Value)
                    .WhereIf(amountReceivedMin.HasValue, e => e.UserPayment.AmountReceived >= amountReceivedMin!.Value)
                    .WhereIf(amountReceivedMax.HasValue, e => e.UserPayment.AmountReceived <= amountReceivedMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(confirmationMethod), e => e.UserPayment.ConfirmationMethod.Contains(confirmationMethod))
                    .WhereIf(status.HasValue, e => e.UserPayment.Status == status)
                    .WhereIf(!string.IsNullOrWhiteSpace(stripPaymentId), e => e.UserPayment.StripPaymentId.Contains(stripPaymentId))
                    .WhereIf(!string.IsNullOrWhiteSpace(stripClientSecret), e => e.UserPayment.StripClientSecret.Contains(stripClientSecret))
                    .WhereIf(!string.IsNullOrWhiteSpace(created), e => e.UserPayment.Created.Contains(created))
                    .WhereIf(userProfileId != null && userProfileId != Guid.Empty, e => e.UserProfile != null && e.UserProfile.Id == userProfileId)
                    .WhereIf(reservationId != null && reservationId != Guid.Empty, e => e.Reservation != null && e.Reservation.Id == reservationId);
        }

        public virtual async Task<List<UserPayment>> GetListAsync(
            string? filterText = null,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            string? description = null,
            string? receiptEmail = null,
            long? amountCapturableMin = null,
            long? amountCapturableMax = null,
            long? amountReceivedMin = null,
            long? amountReceivedMax = null,
            string? confirmationMethod = null,
            UserPaymentStatus? status = null,
            string? stripPaymentId = null,
            string? stripClientSecret = null,
            string? created = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, amountMin, amountMax, currency, description, receiptEmail, amountCapturableMin, amountCapturableMax, amountReceivedMin, amountReceivedMax, confirmationMethod, status, stripPaymentId, stripClientSecret, created);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? UserPaymentConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            string? description = null,
            string? receiptEmail = null,
            long? amountCapturableMin = null,
            long? amountCapturableMax = null,
            long? amountReceivedMin = null,
            long? amountReceivedMax = null,
            string? confirmationMethod = null,
            UserPaymentStatus? status = null,
            string? stripPaymentId = null,
            string? stripClientSecret = null,
            string? created = null,
            Guid? userProfileId = null,
            Guid? reservationId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, amountMin, amountMax, currency, description, receiptEmail, amountCapturableMin, amountCapturableMax, amountReceivedMin, amountReceivedMax, confirmationMethod, status, stripPaymentId, stripClientSecret, created, userProfileId, reservationId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<UserPayment> ApplyFilter(
            IQueryable<UserPayment> query,
            string? filterText = null,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            string? description = null,
            string? receiptEmail = null,
            long? amountCapturableMin = null,
            long? amountCapturableMax = null,
            long? amountReceivedMin = null,
            long? amountReceivedMax = null,
            string? confirmationMethod = null,
            UserPaymentStatus? status = null,
            string? stripPaymentId = null,
            string? stripClientSecret = null,
            string? created = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Currency!.Contains(filterText!) || e.Description!.Contains(filterText!) || e.ReceiptEmail!.Contains(filterText!) || e.ConfirmationMethod!.Contains(filterText!) || e.StripPaymentId!.Contains(filterText!) || e.StripClientSecret!.Contains(filterText!) || e.Created!.Contains(filterText!))
                    .WhereIf(amountMin.HasValue, e => e.Amount >= amountMin!.Value)
                    .WhereIf(amountMax.HasValue, e => e.Amount <= amountMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(currency), e => e.Currency.Contains(currency))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description))
                    .WhereIf(!string.IsNullOrWhiteSpace(receiptEmail), e => e.ReceiptEmail.Contains(receiptEmail))
                    .WhereIf(amountCapturableMin.HasValue, e => e.AmountCapturable >= amountCapturableMin!.Value)
                    .WhereIf(amountCapturableMax.HasValue, e => e.AmountCapturable <= amountCapturableMax!.Value)
                    .WhereIf(amountReceivedMin.HasValue, e => e.AmountReceived >= amountReceivedMin!.Value)
                    .WhereIf(amountReceivedMax.HasValue, e => e.AmountReceived <= amountReceivedMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(confirmationMethod), e => e.ConfirmationMethod.Contains(confirmationMethod))
                    .WhereIf(status.HasValue, e => e.Status == status)
                    .WhereIf(!string.IsNullOrWhiteSpace(stripPaymentId), e => e.StripPaymentId.Contains(stripPaymentId))
                    .WhereIf(!string.IsNullOrWhiteSpace(stripClientSecret), e => e.StripClientSecret.Contains(stripClientSecret))
                    .WhereIf(!string.IsNullOrWhiteSpace(created), e => e.Created.Contains(created));
        }
    }
}