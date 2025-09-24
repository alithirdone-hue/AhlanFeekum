using AhlanFeekum.Governorates;
using AhlanFeekum.PropertyEvaluations;
using AhlanFeekum.PropertyFeatures;
using AhlanFeekum.PropertyMedias;
using AhlanFeekum.PropertyTypes;
using AhlanFeekum.SiteProperties;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace AhlanFeekum.UserProfiles
{
    public  class UserProfileWithDetails
    {
        public UserProfile UserProfile { get; set; } = null!;

        public List<SitePropertyWithDetails> FavoriteProperties { get; set; }
        public List<SitePropertyWithDetails> MyProperties { get; set; }
    }
}