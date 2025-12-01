using AhlanFeekum.Reservations;
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
using AhlanFeekum.PropertyMedias;

namespace AhlanFeekum.Reservations
{
    public abstract class EfCoreReservationRepositoryBase : EfCoreRepository<AhlanFeekumDbContext, Reservation, Guid>
    {
        public EfCoreReservationRepositoryBase(IDbContextProvider<AhlanFeekumDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        DateOnly? fromeDateMin = null,
            DateOnly? fromeDateMax = null,
            DateOnly? toDateMin = null,
            DateOnly? toDateMax = null,
            DateTime? checkInDateMin = null,
            DateTime? checkInDateMax = null,
            DateTime? checkOutDateMin = null,
            DateTime? checkOutDateMax = null,
            int? numberOfGuestMin = null,
            int? numberOfGuestMax = null,
            double? priceMin = null,
            double? priceMax = null,
            double? discountMin = null,
            double? discountMax = null,
            ReservationStatus? reservationStatus = null,
            string? notes = null,
            ReservationPaymentMethod? reservationPaymentMethod = null,
            bool? isPaid = null,
            string? description = null,
            Guid? userProfileId = null,
            Guid? sitePropertyId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();

            query = ApplyFilter(query, filterText, fromeDateMin, fromeDateMax, toDateMin, toDateMax, checkInDateMin, checkInDateMax, checkOutDateMin, checkOutDateMax, numberOfGuestMin, numberOfGuestMax, priceMin, priceMax, discountMin, discountMax, reservationStatus, notes, reservationPaymentMethod, isPaid, description, userProfileId, sitePropertyId);

            var ids = query.Select(x => x.Reservation.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<ReservationWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(reservation => new ReservationWithNavigationProperties
                {
                    Reservation = reservation,
                    UserProfile = dbContext.Set<UserProfile>().FirstOrDefault(c => c.Id == reservation.UserProfileId),
                    SiteProperty = dbContext.Set<SiteProperty>().FirstOrDefault(c => c.Id == reservation.SitePropertyId),
                    PropertyOwner = (from siteProperty in dbContext.Set<SiteProperty>()
                                     where siteProperty.Id == reservation.SitePropertyId
                                     join propertyOwner in dbContext.Set<UserProfile>()
                                          on siteProperty.OwnerId equals propertyOwner.Id
                                     select propertyOwner).FirstOrDefault(),
                    PropertyMedia = dbContext.Set<PropertyMedia>()
                                       .Where(pm => pm.SitePropertyId == reservation.SitePropertyId)
                                       .OrderBy(pm => pm.Order)           // <-- order column
                                       .FirstOrDefault(),
                }).FirstOrDefault();
        }

        public virtual async Task<List<ReservationWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            DateOnly? fromeDateMin = null,
            DateOnly? fromeDateMax = null,
            DateOnly? toDateMin = null,
            DateOnly? toDateMax = null,
            DateTime? checkInDateMin = null,
            DateTime? checkInDateMax = null,
            DateTime? checkOutDateMin = null,
            DateTime? checkOutDateMax = null,
            int? numberOfGuestMin = null,
            int? numberOfGuestMax = null,
            double? priceMin = null,
            double? priceMax = null,
            double? discountMin = null,
            double? discountMax = null,
            ReservationStatus? reservationStatus = null,
            string? notes = null,
            ReservationPaymentMethod? reservationPaymentMethod = null,
            bool? isPaid = null,
            string? description = null,
            Guid? userProfileId = null,
            Guid? sitePropertyId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, fromeDateMin, fromeDateMax, toDateMin, toDateMax, checkInDateMin, checkInDateMax, checkOutDateMin, checkOutDateMax, numberOfGuestMin, numberOfGuestMax, priceMin, priceMax, discountMin, discountMax, reservationStatus, notes, reservationPaymentMethod, isPaid, description, userProfileId, sitePropertyId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ReservationConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<ReservationWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            var dbContext = await GetDbContextAsync();
            return from reservation in (await GetDbSetAsync())
                   join userProfile in (await GetDbContextAsync()).Set<UserProfile>() on reservation.UserProfileId equals userProfile.Id into userProfiles
                   from userProfile in userProfiles.DefaultIfEmpty()
                   join siteProperty in (await GetDbContextAsync()).Set<SiteProperty>() on reservation.SitePropertyId equals siteProperty.Id into siteProperties
                   from siteProperty in siteProperties.DefaultIfEmpty()
                   join propertyOwner in (await GetDbContextAsync()).Set<UserProfile>() on siteProperty.OwnerId equals propertyOwner.Id into propertyOwners
                   from propertyOwner in propertyOwners.DefaultIfEmpty()
                   select new ReservationWithNavigationProperties
                   {
                       Reservation = reservation,
                       UserProfile = userProfile,
                       SiteProperty = siteProperty,
                       PropertyOwner = propertyOwner,
                       PropertyMedia = dbContext.Set<PropertyMedia>()
                                       .Where(pm => pm.SitePropertyId == siteProperty.Id)
                                       .OrderBy(pm => pm.Order)           // <-- order column
                                       .FirstOrDefault(),
                   };
        }

        protected virtual IQueryable<ReservationWithNavigationProperties> ApplyFilter(
            IQueryable<ReservationWithNavigationProperties> query,
            string? filterText,
            DateOnly? fromeDateMin = null,
            DateOnly? fromeDateMax = null,
            DateOnly? toDateMin = null,
            DateOnly? toDateMax = null,
            DateTime? checkInDateMin = null,
            DateTime? checkInDateMax = null,
            DateTime? checkOutDateMin = null,
            DateTime? checkOutDateMax = null,
            int? numberOfGuestMin = null,
            int? numberOfGuestMax = null,
            double? priceMin = null,
            double? priceMax = null,
            double? discountMin = null,
            double? discountMax = null,
            ReservationStatus? reservationStatus = null,
            string? notes = null,
            ReservationPaymentMethod? reservationPaymentMethod = null,
            bool? isPaid = null,
            string? description = null,
            Guid? userProfileId = null,
            Guid? sitePropertyId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Reservation.Notes!.Contains(filterText!) || e.Reservation.Description!.Contains(filterText!))
                    .WhereIf(fromeDateMin.HasValue, e => e.Reservation.FromeDate >= fromeDateMin!.Value)
                    .WhereIf(fromeDateMax.HasValue, e => e.Reservation.FromeDate <= fromeDateMax!.Value)
                    .WhereIf(toDateMin.HasValue, e => e.Reservation.ToDate >= toDateMin!.Value)
                    .WhereIf(toDateMax.HasValue, e => e.Reservation.ToDate <= toDateMax!.Value)
                    .WhereIf(checkInDateMin.HasValue, e => e.Reservation.CheckInDate >= checkInDateMin!.Value)
                    .WhereIf(checkInDateMax.HasValue, e => e.Reservation.CheckInDate <= checkInDateMax!.Value)
                    .WhereIf(checkOutDateMin.HasValue, e => e.Reservation.CheckOutDate >= checkOutDateMin!.Value)
                    .WhereIf(checkOutDateMax.HasValue, e => e.Reservation.CheckOutDate <= checkOutDateMax!.Value)
                    .WhereIf(numberOfGuestMin.HasValue, e => e.Reservation.NumberOfGuest >= numberOfGuestMin!.Value)
                    .WhereIf(numberOfGuestMax.HasValue, e => e.Reservation.NumberOfGuest <= numberOfGuestMax!.Value)
                    .WhereIf(priceMin.HasValue, e => e.Reservation.Price >= priceMin!.Value)
                    .WhereIf(priceMax.HasValue, e => e.Reservation.Price <= priceMax!.Value)
                    .WhereIf(discountMin.HasValue, e => e.Reservation.Discount >= discountMin!.Value)
                    .WhereIf(discountMax.HasValue, e => e.Reservation.Discount <= discountMax!.Value)
                    .WhereIf(reservationStatus.HasValue, e => e.Reservation.ReservationStatus == reservationStatus)
                    .WhereIf(!string.IsNullOrWhiteSpace(notes), e => e.Reservation.Notes.Contains(notes))
                    .WhereIf(reservationPaymentMethod.HasValue, e => e.Reservation.ReservationPaymentMethod == reservationPaymentMethod)
                    .WhereIf(isPaid.HasValue, e => e.Reservation.IsPaid == isPaid)
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Reservation.Description.Contains(description))
                    .WhereIf(userProfileId != null && userProfileId != Guid.Empty, e => e.UserProfile != null && e.UserProfile.Id == userProfileId)
                    .WhereIf(sitePropertyId != null && sitePropertyId != Guid.Empty, e => e.SiteProperty != null && e.SiteProperty.Id == sitePropertyId);
        }

        public virtual async Task<List<Reservation>> GetListAsync(
            string? filterText = null,
            DateOnly? fromeDateMin = null,
            DateOnly? fromeDateMax = null,
            DateOnly? toDateMin = null,
            DateOnly? toDateMax = null,
            DateTime? checkInDateMin = null,
            DateTime? checkInDateMax = null,
            DateTime? checkOutDateMin = null,
            DateTime? checkOutDateMax = null,
            int? numberOfGuestMin = null,
            int? numberOfGuestMax = null,
            double? priceMin = null,
            double? priceMax = null,
            double? discountMin = null,
            double? discountMax = null,
            ReservationStatus? reservationStatus = null,
            string? notes = null,
            ReservationPaymentMethod? reservationPaymentMethod = null,
            bool? isPaid = null,
            string? description = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, fromeDateMin, fromeDateMax, toDateMin, toDateMax, checkInDateMin, checkInDateMax, checkOutDateMin, checkOutDateMax, numberOfGuestMin, numberOfGuestMax, priceMin, priceMax, discountMin, discountMax, reservationStatus, notes, reservationPaymentMethod, isPaid, description);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ReservationConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            DateOnly? fromeDateMin = null,
            DateOnly? fromeDateMax = null,
            DateOnly? toDateMin = null,
            DateOnly? toDateMax = null,
            DateTime? checkInDateMin = null,
            DateTime? checkInDateMax = null,
            DateTime? checkOutDateMin = null,
            DateTime? checkOutDateMax = null,
            int? numberOfGuestMin = null,
            int? numberOfGuestMax = null,
            double? priceMin = null,
            double? priceMax = null,
            double? discountMin = null,
            double? discountMax = null,
            ReservationStatus? reservationStatus = null,
            string? notes = null,
            ReservationPaymentMethod? reservationPaymentMethod = null,
            bool? isPaid = null,
            string? description = null,
            Guid? userProfileId = null,
            Guid? sitePropertyId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, fromeDateMin, fromeDateMax, toDateMin, toDateMax, checkInDateMin, checkInDateMax, checkOutDateMin, checkOutDateMax, numberOfGuestMin, numberOfGuestMax, priceMin, priceMax, discountMin, discountMax, reservationStatus, notes, reservationPaymentMethod, isPaid, description, userProfileId, sitePropertyId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Reservation> ApplyFilter(
            IQueryable<Reservation> query,
            string? filterText = null,
            DateOnly? fromeDateMin = null,
            DateOnly? fromeDateMax = null,
            DateOnly? toDateMin = null,
            DateOnly? toDateMax = null,
            DateTime? checkInDateMin = null,
            DateTime? checkInDateMax = null,
            DateTime? checkOutDateMin = null,
            DateTime? checkOutDateMax = null,
            int? numberOfGuestMin = null,
            int? numberOfGuestMax = null,
            double? priceMin = null,
            double? priceMax = null,
            double? discountMin = null,
            double? discountMax = null,
            ReservationStatus? reservationStatus = null,
            string? notes = null,
            ReservationPaymentMethod? reservationPaymentMethod = null,
            bool? isPaid = null,
            string? description = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Notes!.Contains(filterText!) || e.Description!.Contains(filterText!))
                    .WhereIf(fromeDateMin.HasValue, e => e.FromeDate >= fromeDateMin!.Value)
                    .WhereIf(fromeDateMax.HasValue, e => e.FromeDate <= fromeDateMax!.Value)
                    .WhereIf(toDateMin.HasValue, e => e.ToDate >= toDateMin!.Value)
                    .WhereIf(toDateMax.HasValue, e => e.ToDate <= toDateMax!.Value)
                    .WhereIf(checkInDateMin.HasValue, e => e.CheckInDate >= checkInDateMin!.Value)
                    .WhereIf(checkInDateMax.HasValue, e => e.CheckInDate <= checkInDateMax!.Value)
                    .WhereIf(checkOutDateMin.HasValue, e => e.CheckOutDate >= checkOutDateMin!.Value)
                    .WhereIf(checkOutDateMax.HasValue, e => e.CheckOutDate <= checkOutDateMax!.Value)
                    .WhereIf(numberOfGuestMin.HasValue, e => e.NumberOfGuest >= numberOfGuestMin!.Value)
                    .WhereIf(numberOfGuestMax.HasValue, e => e.NumberOfGuest <= numberOfGuestMax!.Value)
                    .WhereIf(priceMin.HasValue, e => e.Price >= priceMin!.Value)
                    .WhereIf(priceMax.HasValue, e => e.Price <= priceMax!.Value)
                    .WhereIf(discountMin.HasValue, e => e.Discount >= discountMin!.Value)
                    .WhereIf(discountMax.HasValue, e => e.Discount <= discountMax!.Value)
                    .WhereIf(reservationStatus.HasValue, e => e.ReservationStatus == reservationStatus)
                    .WhereIf(!string.IsNullOrWhiteSpace(notes), e => e.Notes.Contains(notes))
                    .WhereIf(reservationPaymentMethod.HasValue, e => e.ReservationPaymentMethod == reservationPaymentMethod)
                    .WhereIf(isPaid.HasValue, e => e.IsPaid == isPaid)
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description));
        }
    }
}