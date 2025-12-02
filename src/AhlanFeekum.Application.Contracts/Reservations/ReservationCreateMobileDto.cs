using AhlanFeekum.Reservations;
using AhlanFeekum.UserPayments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.Reservations
{
    public class ReservationCreateMobileDto
    {
        public DateOnly FromeDate { get; set; }
        public DateOnly ToDate { get; set; }
        //public DateTime? CheckInDate { get; set; }
        //public DateTime? CheckOutDate { get; set; }
        public int? NumberOfGuest { get; set; }
        public string? Notes { get; set; }
        //public Guid UserProfileId { get; set; }
        public Guid SitePropertyId { get; set; }

        public ReservationPaymentMethod PaymentMethod { get; set; } = ((ReservationPaymentMethod[])Enum.GetValues(typeof(ReservationPaymentMethod)))[0];
    }
}