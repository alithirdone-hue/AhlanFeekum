using AhlanFeekum.UserProfiles;
using AhlanFeekum.SiteProperties;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace AhlanFeekum.FavoriteProperties
{
    public abstract class FavoritePropertyBase : FullAuditedAggregateRoot<Guid>
    {

        public Guid UserProfileId { get; set; }
        public Guid SitePropertyId { get; set; }

        protected FavoritePropertyBase()
        {

        }

        public FavoritePropertyBase(Guid id, Guid userProfileId, Guid sitePropertyId)
        {

            Id = id;
            UserProfileId = userProfileId;
            SitePropertyId = sitePropertyId;
        }

    }
}