using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.UserProfiles
{
    public class VerifyRequestDto
    {
        [Required]
        [EmailAddress]
        public string EmailOrPhone { get; set; } = null!;

        [Required]
        public string SecurityCode { get; set; } = null!;
    }
} 