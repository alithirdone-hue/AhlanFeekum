using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.SpecialAdvertisments
{
    public abstract class SpecialAdvertismentCreateDtoBase
    {
        public Guid ImageId { get; set; }
        [Required]
        public string ImageExtension { get; set; } = null!;
        public int Order { get; set; }
        public bool IsActive { get; set; } = true;
        public Guid SitePropertyId { get; set; }
    }
}