using AhlanFeekum.UserProfiles;
using AhlanFeekum.SiteProperties;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace AhlanFeekum.Reservations
{
    public abstract class ReservationWithNavigationPropertiesDtoBase
    {
        public ReservationDto Reservation { get; set; } = null!;

        public UserProfileDto UserProfile { get; set; } = null!;
        public SitePropertyDto SiteProperty { get; set; } = null!;

    }
}