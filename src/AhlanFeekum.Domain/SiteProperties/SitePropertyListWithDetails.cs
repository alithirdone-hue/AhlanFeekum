using AhlanFeekum.Governorates;
using AhlanFeekum.PropertyFeatures;
using AhlanFeekum.PropertyMedias;
using AhlanFeekum.PropertyTypes;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace AhlanFeekum.SiteProperties
{
    public  class SitePropertyListWithDetails 
    {
        public List<SitePropertyWithDetails> SitePropertyWithDetails { get; set; } = null!;
        public long TotalCount { get; set; } = 0;

   
    }
}