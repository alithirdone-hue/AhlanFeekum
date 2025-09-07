using AhlanFeekum.SiteProperties;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace AhlanFeekum.SpecialAdvertisments
{
    public abstract class SpecialAdvertismentWithNavigationPropertiesDtoBase
    {
        public SpecialAdvertismentDto SpecialAdvertisment { get; set; } = null!;

        public SitePropertyDto SiteProperty { get; set; } = null!;

    }
}