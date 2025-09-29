using Volo.Abp.Application.Dtos;
using System;

namespace AhlanFeekum.AhlanfeekumTerms
{
    public abstract class AhlanfeekumTermExcelDownloadDtoBase
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? TermsTitle { get; set; }
        public string? TermsAnnotation { get; set; }
        public string? TermsDescription { get; set; }
        public string? TermsIconExtension { get; set; }
        public string? WhoAreWeTitle { get; set; }
        public string? WhoAreWeAnnotation { get; set; }
        public string? WhoAreWeDescription { get; set; }
        public string? WhoAreWeIconExtension { get; set; }
        public bool? IsActive { get; set; }

        public AhlanfeekumTermExcelDownloadDtoBase()
        {

        }
    }
}