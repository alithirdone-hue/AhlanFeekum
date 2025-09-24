using System.Collections.Generic;
using System;
using AhlanFeekum.SiteProperties;
namespace AhlanFeekum.UserProfiles
{
    public class UserProfileWithDetailsMobileDto 
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Address { get; set; }
        public string? ProfilePhoto { get; set; }
        public bool IsSuperHost { get; set; }

        public string? RoleId { get; set; } = null;

        public double SpeedOfCompletion { get; set; }
        public double Dealing { get; set; }
        public double Cleanliness { get; set; }
        public double Perfection { get; set; }
        public double Price { get; set; }

        public List<SitePropertyListingMobileDto> FavoriteProperties { get; set; }
        public List<SitePropertyListingMobileDto> MyProperties { get; set; }
    }
}