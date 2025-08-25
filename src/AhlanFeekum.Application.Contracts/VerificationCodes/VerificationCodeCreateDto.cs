using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.VerificationCodes
{
    public abstract class VerificationCodeCreateDtoBase
    {
        [Required]
        public string PhoneOrEmail { get; set; } = null!;
        public int SecurityCode { get; set; }
        public bool IsExpired { get; set; } = false;
    }
}