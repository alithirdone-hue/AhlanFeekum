using AhlanFeekum.CashPayments;
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

namespace AhlanFeekum.CashPayments
{
    public abstract class EfCoreCashPaymentRepositoryBase : EfCoreRepository<AhlanFeekumDbContext, CashPayment, Guid>
    {
        public EfCoreCashPaymentRepositoryBase(IDbContextProvider<AhlanFeekumDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            DateTime? paymentDateMin = null,
            DateTime? paymentDateMax = null,
            string? description = null,
            CashPaymentStatus? status = null,
            Guid? userProfileId = null,
            Guid? reservationId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();

            query = ApplyFilter(query, filterText, amountMin, amountMax, currency, paymentDateMin, paymentDateMax, description, status, userProfileId, reservationId);

            var ids = query.Select(x => x.CashPayment.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<CashPaymentWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(cashPayment => new CashPaymentWithNavigationProperties
                {
                    CashPayment = cashPayment,
                    UserProfile = dbContext.Set<UserProfile>().FirstOrDefault(c => c.Id == cashPayment.UserProfileId),
                    Reservation = dbContext.Set<Reservation>().FirstOrDefault(c => c.Id == cashPayment.ReservationId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<CashPaymentWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            DateTime? paymentDateMin = null,
            DateTime? paymentDateMax = null,
            string? description = null,
            CashPaymentStatus? status = null,
            Guid? userProfileId = null,
            Guid? reservationId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, amountMin, amountMax, currency, paymentDateMin, paymentDateMax, description, status, userProfileId, reservationId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CashPaymentConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<CashPaymentWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from cashPayment in (await GetDbSetAsync())
                   join userProfile in (await GetDbContextAsync()).Set<UserProfile>() on cashPayment.UserProfileId equals userProfile.Id into userProfiles
                   from userProfile in userProfiles.DefaultIfEmpty()
                   join reservation in (await GetDbContextAsync()).Set<Reservation>() on cashPayment.ReservationId equals reservation.Id into reservations
                   from reservation in reservations.DefaultIfEmpty()
                   select new CashPaymentWithNavigationProperties
                   {
                       CashPayment = cashPayment,
                       UserProfile = userProfile,
                       Reservation = reservation
                   };
        }

        protected virtual IQueryable<CashPaymentWithNavigationProperties> ApplyFilter(
            IQueryable<CashPaymentWithNavigationProperties> query,
            string? filterText,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            DateTime? paymentDateMin = null,
            DateTime? paymentDateMax = null,
            string? description = null,
            CashPaymentStatus? status = null,
            Guid? userProfileId = null,
            Guid? reservationId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.CashPayment.Currency!.Contains(filterText!) || e.CashPayment.Description!.Contains(filterText!))
                    .WhereIf(amountMin.HasValue, e => e.CashPayment.Amount >= amountMin!.Value)
                    .WhereIf(amountMax.HasValue, e => e.CashPayment.Amount <= amountMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(currency), e => e.CashPayment.Currency.Contains(currency))
                    .WhereIf(paymentDateMin.HasValue, e => e.CashPayment.PaymentDate >= paymentDateMin!.Value)
                    .WhereIf(paymentDateMax.HasValue, e => e.CashPayment.PaymentDate <= paymentDateMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.CashPayment.Description.Contains(description))
                    .WhereIf(status.HasValue, e => e.CashPayment.Status == status)
                    .WhereIf(userProfileId != null && userProfileId != Guid.Empty, e => e.UserProfile != null && e.UserProfile.Id == userProfileId)
                    .WhereIf(reservationId != null && reservationId != Guid.Empty, e => e.Reservation != null && e.Reservation.Id == reservationId);
        }

        public virtual async Task<List<CashPayment>> GetListAsync(
            string? filterText = null,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            DateTime? paymentDateMin = null,
            DateTime? paymentDateMax = null,
            string? description = null,
            CashPaymentStatus? status = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, amountMin, amountMax, currency, paymentDateMin, paymentDateMax, description, status);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CashPaymentConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            DateTime? paymentDateMin = null,
            DateTime? paymentDateMax = null,
            string? description = null,
            CashPaymentStatus? status = null,
            Guid? userProfileId = null,
            Guid? reservationId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, amountMin, amountMax, currency, paymentDateMin, paymentDateMax, description, status, userProfileId, reservationId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<CashPayment> ApplyFilter(
            IQueryable<CashPayment> query,
            string? filterText = null,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            DateTime? paymentDateMin = null,
            DateTime? paymentDateMax = null,
            string? description = null,
            CashPaymentStatus? status = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Currency!.Contains(filterText!) || e.Description!.Contains(filterText!))
                    .WhereIf(amountMin.HasValue, e => e.Amount >= amountMin!.Value)
                    .WhereIf(amountMax.HasValue, e => e.Amount <= amountMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(currency), e => e.Currency.Contains(currency))
                    .WhereIf(paymentDateMin.HasValue, e => e.PaymentDate >= paymentDateMin!.Value)
                    .WhereIf(paymentDateMax.HasValue, e => e.PaymentDate <= paymentDateMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description))
                    .WhereIf(status.HasValue, e => e.Status == status);
        }
    }
}