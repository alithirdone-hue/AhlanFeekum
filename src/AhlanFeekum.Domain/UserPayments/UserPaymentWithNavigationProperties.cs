using AhlanFeekum.UserProfiles;
using AhlanFeekum.Reservations;

using System;
using System.Collections.Generic;

namespace AhlanFeekum.UserPayments
{
    public abstract class UserPaymentWithNavigationPropertiesBase
    {
        public UserPayment UserPayment { get; set; } = null!;

        public UserProfile UserProfile { get; set; } = null!;
        public Reservation Reservation { get; set; } = null!;
        

        
    }
}