using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.Tickets
{
    public abstract class TicketUpdateDtoBase : IHasConcurrencyStamp
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        public bool IsFixed { get; set; }
        public Guid? UserProfileId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}