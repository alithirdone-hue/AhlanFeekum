using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.PropertyMedias
{
    public  class PropertyMediaCreateMobileDto
    {
        public List<PropertyMediaItemDto> propertyMediaItemDtos = new List<PropertyMediaItemDto>();
        public Guid SitePropertyId { get; set; }
    }
}