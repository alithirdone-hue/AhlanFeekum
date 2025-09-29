using AhlanFeekum.UserProfiles;

using System;
using System.Collections.Generic;

namespace AhlanFeekum.Tickets
{
    public abstract class TicketWithNavigationPropertiesBase
    {
        public Ticket Ticket { get; set; } = null!;

        public UserProfile UserProfile { get; set; } = null!;
        

        
    }
}