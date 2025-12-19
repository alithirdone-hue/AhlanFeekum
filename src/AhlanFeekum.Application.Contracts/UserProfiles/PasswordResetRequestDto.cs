using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.UserProfiles
{
    public class PasswordResetRequestDto
    {
        [Required]
        public string EmailOrPhone { get; set; } = null!;

    }
} 