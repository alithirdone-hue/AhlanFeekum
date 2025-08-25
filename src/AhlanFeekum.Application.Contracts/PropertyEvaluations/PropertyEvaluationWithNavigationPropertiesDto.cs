using AhlanFeekum.UserProfiles;
using AhlanFeekum.SiteProperties;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace AhlanFeekum.PropertyEvaluations
{
    public abstract class PropertyEvaluationWithNavigationPropertiesDtoBase
    {
        public PropertyEvaluationDto PropertyEvaluation { get; set; } = null!;

        public UserProfileDto UserProfile { get; set; } = null!;
        public SitePropertyDto SiteProperty { get; set; } = null!;

    }
}