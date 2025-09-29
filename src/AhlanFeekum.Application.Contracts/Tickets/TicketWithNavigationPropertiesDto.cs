using AhlanFeekum.UserProfiles;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace AhlanFeekum.Tickets
{
    public abstract class TicketWithNavigationPropertiesDtoBase
    {
        public TicketDto Ticket { get; set; } = null!;

        public UserProfileDto UserProfile { get; set; } = null!;

    }
}