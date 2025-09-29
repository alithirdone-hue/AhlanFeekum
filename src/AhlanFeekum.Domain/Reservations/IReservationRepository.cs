using AhlanFeekum.Reservations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace AhlanFeekum.Reservations
{
    public partial interface IReservationRepository : IRepository<Reservation, Guid>
    {

        Task DeleteAllAsync(
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
            Guid? userProfileId = null,
            Guid? sitePropertyId = null,
            CancellationToken cancellationToken = default);
        Task<ReservationWithNavigationProperties> GetWithNavigationPropertiesAsync(
            Guid id,
            CancellationToken cancellationToken = default
        );

        Task<List<ReservationWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
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
            Guid? userProfileId = null,
            Guid? sitePropertyId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<Reservation>> GetListAsync(
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
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
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
            Guid? userProfileId = null,
            Guid? sitePropertyId = null,
            CancellationToken cancellationToken = default);
    }
}