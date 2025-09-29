using AhlanFeekum.UserProfiles;
using AhlanFeekum.SiteProperties;

using System;
using System.Collections.Generic;
using AhlanFeekum.PropertyMedias;

namespace AhlanFeekum.Reservations
{
    public abstract class ReservationWithNavigationPropertiesBase
    {
        public Reservation Reservation { get; set; } = null!;

        public UserProfile UserProfile { get; set; } = null!;
        public UserProfile? PropertyOwner { get; set; } = null!;
        public SiteProperty SiteProperty { get; set; } = null!;
        public PropertyMedia PropertyMedia { get; set; } = null!;
        

        
    }
}