using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.SpecialAdvertisments
{
    public class SpecialAdvertismentMobileDto 
    {
        public Guid Id { get; set; }
        public string Image { get; set; } = null!;
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public Guid SitePropertyId { get; set; }
        public string SitePropertyTitle { get; set; }

    }
}