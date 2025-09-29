using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.AhlanfeekumTerms
{
    public class AhlanfeekumTermMobileDto 
    {
        public Guid Id { get; set; }
        public string TermsTitle { get; set; } = null!;
        public string TermsAnnotation { get; set; } = null!;
        public string TermsDescription { get; set; } = null!;
        public Guid TermsIcon { get; set; }
        public string WhoAreWeTitle { get; set; } = null!;
        public string? WhoAreWeAnnotation { get; set; }
        public string WhoAreWeDescription { get; set; } = null!;
        public Guid WhoAreWeIcon { get; set; }
        public bool IsActive { get; set; }


    }
}