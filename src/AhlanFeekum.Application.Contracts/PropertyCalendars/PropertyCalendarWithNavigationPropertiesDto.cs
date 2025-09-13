using AhlanFeekum.SiteProperties;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace AhlanFeekum.PropertyCalendars
{
    public abstract class PropertyCalendarWithNavigationPropertiesDtoBase
    {
        public PropertyCalendarDto PropertyCalendar { get; set; } = null!;

        public SitePropertyDto SiteProperty { get; set; } = null!;

    }
}