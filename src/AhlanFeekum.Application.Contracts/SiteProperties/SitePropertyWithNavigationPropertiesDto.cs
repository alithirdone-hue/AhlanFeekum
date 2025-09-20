using AhlanFeekum.PropertyTypes;
using AhlanFeekum.Governorates;
using AhlanFeekum.UserProfiles;
using AhlanFeekum.PropertyFeatures;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace AhlanFeekum.SiteProperties
{
    public abstract class SitePropertyWithNavigationPropertiesDtoBase
    {
        public SitePropertyDto SiteProperty { get; set; } = null!;

        public PropertyTypeDto PropertyType { get; set; } = null!;
        public GovernorateDto Governorate { get; set; } = null!;
        public UserProfileDto Owner { get; set; } = null!;
        public List<PropertyFeatureDto> PropertyFeatures { get; set; } = new List<PropertyFeatureDto>();

    }
}