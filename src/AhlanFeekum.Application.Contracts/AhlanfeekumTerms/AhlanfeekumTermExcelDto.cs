using System;

namespace AhlanFeekum.AhlanfeekumTerms
{
    public abstract class AhlanfeekumTermExcelDtoBase
    {
        public string TermsTitle { get; set; } = null!;
        public string TermsAnnotation { get; set; } = null!;
        public string TermsDescription { get; set; } = null!;
        public string TermsIconExtension { get; set; } = null!;
        public string WhoAreWeTitle { get; set; } = null!;
        public string? WhoAreWeAnnotation { get; set; }
        public string WhoAreWeDescription { get; set; } = null!;
        public string WhoAreWeIconExtension { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}