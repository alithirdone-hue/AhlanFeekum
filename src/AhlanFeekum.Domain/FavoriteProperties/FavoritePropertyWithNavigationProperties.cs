using AhlanFeekum.UserProfiles;
using AhlanFeekum.SiteProperties;

using System;
using System.Collections.Generic;

namespace AhlanFeekum.FavoriteProperties
{
    public abstract class FavoritePropertyWithNavigationPropertiesBase
    {
        public FavoriteProperty FavoriteProperty { get; set; } = null!;

        public UserProfile UserProfile { get; set; } = null!;
        public SiteProperty SiteProperty { get; set; } = null!;
        

        
    }
}