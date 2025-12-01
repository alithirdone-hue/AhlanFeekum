using AhlanFeekum.Reservations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.Reservations
{
    public abstract class ReservationCreateDtoBase
    {
        public DateOnly FromeDate { get; set; }
        public DateOnly ToDate { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int? NumberOfGuest { get; set; }
        public double Price { get; set; }
        public double? Discount { get; set; } = 0;
        public ReservationStatus ReservationStatus { get; set; } = ((ReservationStatus[])Enum.GetValues(typeof(ReservationStatus)))[0];
        public string? Notes { get; set; }
        public ReservationPaymentMethod? ReservationPaymentMethod { get; set; }
        public bool IsPaid { get; set; } = false;
        public string? Description { get; set; }
        public Guid UserProfileId { get; set; }
        public Guid SitePropertyId { get; set; }
    }
}