using AhlanFeekum.SiteProperties;

using System;
using System.Collections.Generic;

namespace AhlanFeekum.PropertyMedias
{
    public abstract class PropertyMediaWithNavigationPropertiesBase
    {
        public PropertyMedia PropertyMedia { get; set; } = null!;

        public SiteProperty SiteProperty { get; set; } = null!;
        

        
    }
}