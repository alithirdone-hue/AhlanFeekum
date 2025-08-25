using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.FavoriteProperties
{
    public abstract class FavoritePropertyDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {

        public Guid UserProfileId { get; set; }
        public Guid SitePropertyId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}