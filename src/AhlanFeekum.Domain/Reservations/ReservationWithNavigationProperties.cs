using AhlanFeekum.UserProfiles;
using AhlanFeekum.SiteProperties;

using System;
using System.Collections.Generic;

namespace AhlanFeekum.Reservations
{
    public abstract class ReservationWithNavigationPropertiesBase
    {
        public Reservation Reservation { get; set; } = null!;

        public UserProfile UserProfile { get; set; } = null!;
        public SiteProperty SiteProperty { get; set; } = null!;
        

        
    }
}