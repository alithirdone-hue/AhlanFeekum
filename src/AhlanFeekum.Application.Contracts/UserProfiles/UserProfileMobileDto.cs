using System.Collections.Generic;
using System;
using AhlanFeekum.SiteProperties;
namespace AhlanFeekum.UserProfiles
{
    public class UserProfileMobileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? ProfilePhoto { get; set; }
        public double? AverageRating { get; set; }
        public bool IsSuperHost { get; set; }
    }
}