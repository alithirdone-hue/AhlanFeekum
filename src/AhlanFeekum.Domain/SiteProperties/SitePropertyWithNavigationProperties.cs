using AhlanFeekum.PropertyTypes;
using AhlanFeekum.Governorates;
using AhlanFeekum.PropertyFeatures;

using System;
using System.Collections.Generic;

namespace AhlanFeekum.SiteProperties
{
    public abstract class SitePropertyWithNavigationPropertiesBase
    {
        public SiteProperty SiteProperty { get; set; } = null!;

        public PropertyType PropertyType { get; set; } = null!;
        public Governorate Governorate { get; set; } = null!;
        

        public List<PropertyFeature> PropertyFeatures { get; set; } = null!;
        
    }
}