using AhlanFeekum.Reservations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.Reservations
{
    public abstract class ReservationUpdateDtoBase : IHasConcurrencyStamp
    {
        public DateOnly FromeDate { get; set; }
        public DateOnly ToDate { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int? NumberOfGuest { get; set; }
        public double Price { get; set; }
        public double? Discount { get; set; }
        public ReservationStatus ReservationStatus { get; set; }
        public string? Notes { get; set; }
        public Guid UserProfileId { get; set; }
        public Guid SitePropertyId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}