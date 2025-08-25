using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace AhlanFeekum.UserProfiles
{
    public class RegisterCreateMobileDto
    {
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Address { get; set; }
        public string Password { get; set; } = null!;
        public IFormFile? ProfilePhoto { get; set; }
        public bool IsSuperHost { get; set; }

        [Required]
        public string? RoleId { get; set; } = null!;


    }
}