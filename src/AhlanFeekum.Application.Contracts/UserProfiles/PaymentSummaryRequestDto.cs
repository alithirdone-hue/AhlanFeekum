using System;
using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.UserProfiles
{
    public class PaymentSummaryRequestDto
    {
        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }
    }
}

