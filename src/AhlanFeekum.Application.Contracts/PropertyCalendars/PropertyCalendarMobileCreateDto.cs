using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.PropertyCalendars
{
    public  class PropertyCalendarMobileCreateDto
    {
        public List<PropertyCalendarItemDto> PropertyCalendarItemDtos = new List<PropertyCalendarItemDto>();
        public Guid SitePropertyId { get; set; }
    }
}