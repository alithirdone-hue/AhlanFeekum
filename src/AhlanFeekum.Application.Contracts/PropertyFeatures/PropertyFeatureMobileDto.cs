using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.PropertyFeatures
{
    public class PropertyFeatureMobileDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public int Order { get; set; }
        public bool IsActive { get; set; }

    }
}