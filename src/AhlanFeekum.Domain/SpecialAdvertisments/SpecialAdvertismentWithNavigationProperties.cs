using AhlanFeekum.SiteProperties;

using System;
using System.Collections.Generic;

namespace AhlanFeekum.SpecialAdvertisments
{
    public abstract class SpecialAdvertismentWithNavigationPropertiesBase
    {
        public SpecialAdvertisment SpecialAdvertisment { get; set; } = null!;

        public SiteProperty SiteProperty { get; set; } = null!;
        

        
    }
}