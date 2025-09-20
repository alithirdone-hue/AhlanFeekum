using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.SpecialAdvertisments
{
    public abstract class SpecialAdvertismentUpdateDtoBase : IHasConcurrencyStamp
    {
        public Guid ImageId { get; set; }
        [Required]
        public string ImageExtension { get; set; } = null!;
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public Guid SitePropertyId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}