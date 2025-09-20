using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.OnlyForYouSections
{
    public abstract class OnlyForYouSectionDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public Guid FirstPhotoId { get; set; }
        public Guid SecondPhotoId { get; set; }
        public Guid ThirdPhotoId { get; set; }
        public string FirstPhotoExtension { get; set; } = null!;
        public string SecondPhotoExtension { get; set; } = null!;
        public string ThirdPhotoExtension { get; set; } = null!;

        public string ConcurrencyStamp { get; set; } = null!;

    }
}