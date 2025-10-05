using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.UserNotifications
{
    public abstract class UserNotificationDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;

        public string ConcurrencyStamp { get; set; } = null!;

    }
}