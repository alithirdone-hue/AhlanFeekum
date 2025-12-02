using AhlanFeekum.UserProfiles;
using AhlanFeekum.Reservations;

using System;
using System.Collections.Generic;

namespace AhlanFeekum.CashPayments
{
    public abstract class CashPaymentWithNavigationPropertiesBase
    {
        public CashPayment CashPayment { get; set; } = null!;

        public UserProfile UserProfile { get; set; } = null!;
        public Reservation Reservation { get; set; } = null!;
        

        
    }
}