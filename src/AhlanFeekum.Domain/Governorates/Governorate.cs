using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace AhlanFeekum.Governorates
{
    public abstract class GovernorateBase : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Title { get; set; }

        public virtual Guid IconId { get; set; }

        [NotNull]
        public virtual string iconExtension { get; set; }

        public virtual int Order { get; set; }

        public virtual bool IsActive { get; set; }

        protected GovernorateBase()
        {

        }

        public GovernorateBase(Guid id, string title, Guid iconId, string iconExtension, int order, bool isActive)
        {

            Id = id;
            Check.NotNull(title, nameof(title));
            Check.NotNull(iconExtension, nameof(iconExtension));
            Title = title;
            IconId = iconId;
            this.iconExtension = iconExtension;
            Order = order;
            IsActive = isActive;
        }

    }
}