using AhlanFeekum.UserProfiles;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace AhlanFeekum.Tickets
{
    public abstract class TicketBase : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string FirstName { get; set; }

        [NotNull]
        public virtual string LastName { get; set; }

        [NotNull]
        public virtual string Description { get; set; }

        public virtual bool IsFixed { get; set; }
        public Guid? UserProfileId { get; set; }

        protected TicketBase()
        {

        }

        public TicketBase(Guid id, Guid? userProfileId, string firstName, string lastName, string description, bool isFixed)
        {

            Id = id;
            Check.NotNull(firstName, nameof(firstName));
            Check.NotNull(lastName, nameof(lastName));
            Check.NotNull(description, nameof(description));
            FirstName = firstName;
            LastName = lastName;
            Description = description;
            IsFixed = isFixed;
            UserProfileId = userProfileId;
        }

    }
}