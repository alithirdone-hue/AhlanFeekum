using AhlanFeekum.Reservations;
using System;

namespace AhlanFeekum.Reservations
{
    public abstract class ReservationExcelDtoBase
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
    }
}