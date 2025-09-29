using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.AhlanfeekumTerms
{
    public abstract class AhlanfeekumTermDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string TermsTitle { get; set; } = null!;
        public string TermsAnnotation { get; set; } = null!;
        public string TermsDescription { get; set; } = null!;
        public Guid TermsIconId { get; set; }
        public string TermsIconExtension { get; set; } = null!;
        public string WhoAreWeTitle { get; set; } = null!;
        public string? WhoAreWeAnnotation { get; set; }
        public string WhoAreWeDescription { get; set; } = null!;
        public Guid WhoAreWeIconId { get; set; }
        public string WhoAreWeIconExtension { get; set; } = null!;
        public bool IsActive { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}