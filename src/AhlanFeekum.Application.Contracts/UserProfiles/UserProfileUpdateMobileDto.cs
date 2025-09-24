using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.UserProfiles
{
    public class UserProfileUpdateMobileDto
    {

        [Required]
        public string Name { get; set; } = null!;
        [EmailAddress]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Address { get; set; }
        public IFormFile? ProfilePhoto { get; set; }
        public bool IsProfilePhotoChanged { get; set; } = false;
    }
}