using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.UserProfiles
{
    public class PasswordChangeRequestDto
    {
        [Required]
        [EmailAddress]
        public string EmailOrPhone { get; set; } = null!;

        [Required]
        public string OldPassword { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = null!;
    }
} 