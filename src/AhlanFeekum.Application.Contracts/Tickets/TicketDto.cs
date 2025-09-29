using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.Tickets
{
    public abstract class TicketDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsFixed { get; set; }
        public Guid? UserProfileId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}