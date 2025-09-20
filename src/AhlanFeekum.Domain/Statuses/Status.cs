using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace AhlanFeekum.Statuses
{
    public abstract class StatusBase : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Name { get; set; }

        public virtual int Order { get; set; }

        public virtual bool IsActive { get; set; }

        protected StatusBase()
        {

        }

        public StatusBase(Guid id, string name, int order, bool isActive)
        {

            Id = id;
            Check.NotNull(name, nameof(name));
            Name = name;
            Order = order;
            IsActive = isActive;
        }

    }
}