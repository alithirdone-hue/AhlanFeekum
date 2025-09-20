using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.OnlyForYouSections
{
    public abstract class OnlyForYouSectionUpdateDtoBase : IHasConcurrencyStamp
    {
        public Guid FirstPhotoId { get; set; }
        public Guid SecondPhotoId { get; set; }
        public Guid ThirdPhotoId { get; set; }
        [Required]
        public string FirstPhotoExtension { get; set; } = null!;
        [Required]
        public string SecondPhotoExtension { get; set; } = null!;
        [Required]
        public string ThirdPhotoExtension { get; set; } = null!;

        public string ConcurrencyStamp { get; set; } = null!;
    }
}