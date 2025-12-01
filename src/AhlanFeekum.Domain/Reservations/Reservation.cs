using AhlanFeekum.Reservations;
using AhlanFeekum.UserProfiles;
using AhlanFeekum.SiteProperties;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace AhlanFeekum.Reservations
{
    public abstract class ReservationBase : FullAuditedAggregateRoot<Guid>
    {
        public virtual DateOnly FromeDate { get; set; }

        public virtual DateOnly ToDate { get; set; }

        public virtual DateTime? CheckInDate { get; set; }

        public virtual DateTime? CheckOutDate { get; set; }

        public virtual int? NumberOfGuest { get; set; }

        public virtual double Price { get; set; }

        public virtual double? Discount { get; set; }

        public virtual ReservationStatus ReservationStatus { get; set; }

        [CanBeNull]
        public virtual string? Notes { get; set; }

        public virtual ReservationPaymentMethod? ReservationPaymentMethod { get; set; }

        public virtual bool IsPaid { get; set; }

        [CanBeNull]
        public virtual string? Description { get; set; }
        public Guid UserProfileId { get; set; }
        public Guid SitePropertyId { get; set; }

        protected ReservationBase()
        {

        }

        public ReservationBase(Guid id, Guid userProfileId, Guid sitePropertyId, DateOnly fromeDate, DateOnly toDate, double price, ReservationStatus reservationStatus, bool isPaid, DateTime? checkInDate = null, DateTime? checkOutDate = null, int? numberOfGuest = null, double? discount = null, string? notes = null, ReservationPaymentMethod? reservationPaymentMethod = null, string? description = null)
        {

            Id = id;
            FromeDate = fromeDate;
            ToDate = toDate;
            Price = price;
            ReservationStatus = reservationStatus;
            IsPaid = isPaid;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            NumberOfGuest = numberOfGuest;
            Discount = discount;
            Notes = notes;
            ReservationPaymentMethod = reservationPaymentMethod;
            Description = description;
            UserProfileId = userProfileId;
            SitePropertyId = sitePropertyId;
        }

    }
}