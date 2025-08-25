using AhlanFeekum.SiteProperties;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace AhlanFeekum.PropertyMedias
{
    public abstract class PropertyMediaWithNavigationPropertiesDtoBase
    {
        public PropertyMediaDto PropertyMedia { get; set; } = null!;

        public SitePropertyDto SiteProperty { get; set; } = null!;

    }
}