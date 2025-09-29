using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.Tickets
{
    public abstract class TicketCreateDtoBase
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        public bool IsFixed { get; set; } = false;
        public Guid? UserProfileId { get; set; }
    }
}