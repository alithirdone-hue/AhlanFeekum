using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.PropertyMedias
{
    public  class PropertyMediaMobileDto 
    {
        public Guid Id { get; set; }
        public string Image { get; set; } = null!;
        public int Order { get; set; }
        public bool isActive { get; set; }
        public Guid SitePropertyId { get; set; }

    }
}