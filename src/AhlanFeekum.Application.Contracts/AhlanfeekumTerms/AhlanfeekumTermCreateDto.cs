using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.AhlanfeekumTerms
{
    public abstract class AhlanfeekumTermCreateDtoBase
    {
        [Required]
        public string TermsTitle { get; set; } = null!;
        [Required]
        public string TermsAnnotation { get; set; } = null!;
        [Required]
        public string TermsDescription { get; set; } = null!;
        public Guid TermsIconId { get; set; }
        [Required]
        public string TermsIconExtension { get; set; } = null!;
        [Required]
        public string WhoAreWeTitle { get; set; } = null!;
        public string? WhoAreWeAnnotation { get; set; }
        [Required]
        public string WhoAreWeDescription { get; set; } = null!;
        public Guid WhoAreWeIconId { get; set; }
        [Required]
        public string WhoAreWeIconExtension { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}