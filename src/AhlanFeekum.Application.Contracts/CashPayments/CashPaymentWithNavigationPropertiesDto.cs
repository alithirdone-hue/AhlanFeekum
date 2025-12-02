using AhlanFeekum.UserProfiles;
using AhlanFeekum.Reservations;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace AhlanFeekum.CashPayments
{
    public abstract class CashPaymentWithNavigationPropertiesDtoBase
    {
        public CashPaymentDto CashPayment { get; set; } = null!;

        public UserProfileDto UserProfile { get; set; } = null!;
        public ReservationDto Reservation { get; set; } = null!;

    }
}