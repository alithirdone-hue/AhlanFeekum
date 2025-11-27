using AhlanFeekum.UserProfiles;
using AhlanFeekum.Reservations;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace AhlanFeekum.UserPayments
{
    public abstract class UserPaymentWithNavigationPropertiesDtoBase
    {
        public UserPaymentDto UserPayment { get; set; } = null!;

        public UserProfileDto UserProfile { get; set; } = null!;
        public ReservationDto Reservation { get; set; } = null!;

    }
}