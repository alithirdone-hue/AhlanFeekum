using AhlanFeekum.UserProfiles;
using AhlanFeekum.SiteProperties;

using System;
using System.Collections.Generic;

namespace AhlanFeekum.PropertyEvaluations
{
    public abstract class PropertyEvaluationWithNavigationPropertiesBase
    {
        public PropertyEvaluation PropertyEvaluation { get; set; } = null!;

        public UserProfile UserProfile { get; set; } = null!;
        public SiteProperty SiteProperty { get; set; } = null!;
        

        
    }
}