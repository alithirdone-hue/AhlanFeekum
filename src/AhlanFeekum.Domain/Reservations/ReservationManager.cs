using AhlanFeekum.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace AhlanFeekum.Reservations
{
    public abstract class ReservationManagerBase : DomainService
    {
        protected IReservationRepository _reservationRepository;

        public ReservationManagerBase(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public virtual async Task<Reservation> CreateAsync(
        Guid userProfileId, Guid sitePropertyId, DateOnly fromeDate, DateOnly toDate, double price, ReservationStatus reservationStatus, bool isPaid, DateTime? checkInDate = null, DateTime? checkOutDate = null, int? numberOfGuest = null, double? discount = null, string? notes = null, ReservationPaymentMethod? reservationPaymentMethod = null, string? description = null)
        {
            Check.NotNull(userProfileId, nameof(userProfileId));
            Check.NotNull(sitePropertyId, nameof(sitePropertyId));
            Check.NotNull(reservationStatus, nameof(reservationStatus));

            var reservation = new Reservation(
             GuidGenerator.Create(),
             userProfileId, sitePropertyId, fromeDate, toDate, price, reservationStatus, isPaid, checkInDate, checkOutDate, numberOfGuest, discount, notes, reservationPaymentMethod, description
             );

            return await _reservationRepository.InsertAsync(reservation, autoSave: true);
        }

        public virtual async Task<Reservation> UpdateAsync(
            Guid id,
            Guid userProfileId, Guid sitePropertyId, DateOnly fromeDate, DateOnly toDate, double price, ReservationStatus reservationStatus, bool isPaid, DateTime? checkInDate = null, DateTime? checkOutDate = null, int? numberOfGuest = null, double? discount = null, string? notes = null, ReservationPaymentMethod? reservationPaymentMethod = null, string? description = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNull(userProfileId, nameof(userProfileId));
            Check.NotNull(sitePropertyId, nameof(sitePropertyId));
            Check.NotNull(reservationStatus, nameof(reservationStatus));

            var reservation = await _reservationRepository.GetAsync(id);

            reservation.UserProfileId = userProfileId;
            reservation.SitePropertyId = sitePropertyId;
            reservation.FromeDate = fromeDate;
            reservation.ToDate = toDate;
            reservation.Price = price;
            reservation.ReservationStatus = reservationStatus;
            reservation.IsPaid = isPaid;
            reservation.CheckInDate = checkInDate;
            reservation.CheckOutDate = checkOutDate;
            reservation.NumberOfGuest = numberOfGuest;
            reservation.Discount = discount;
            reservation.Notes = notes;
            reservation.ReservationPaymentMethod = reservationPaymentMethod;
            reservation.Description = description;

            reservation.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _reservationRepository.UpdateAsync(reservation);
        }

    }
}