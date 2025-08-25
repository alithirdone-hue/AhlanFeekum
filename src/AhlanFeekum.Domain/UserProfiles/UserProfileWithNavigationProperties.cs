using Volo.Abp.Identity;
using Volo.Abp.Identity;

using System;
using System.Collections.Generic;

namespace AhlanFeekum.UserProfiles
{
    public abstract class UserProfileWithNavigationPropertiesBase
    {
        public UserProfile UserProfile { get; set; } = null!;

        public IdentityRole IdentityRole { get; set; } = null!;
        public IdentityUser IdentityUser { get; set; } = null!;
        

        
    }
}