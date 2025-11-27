using System;
using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.UserProfiles
{
    public class PaymentSummaryRequestDto
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}

