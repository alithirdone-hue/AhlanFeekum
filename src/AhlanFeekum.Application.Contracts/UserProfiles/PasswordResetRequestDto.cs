using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.UserProfiles
{
    public class PasswordResetRequestDto
    {
        [Required]
        [EmailAddress]
        public string EmailOrPhone { get; set; } = null!;

    }
} 