using AhlanFeekum.Governorates;
using AhlanFeekum.OnlyForYouSections;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.SpecialAdvertisments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AhlanFeekum.Permissions.AhlanFeekumPermissions;

namespace AhlanFeekum.UserProfiles
{
    public class HomePageDto
    {
        public UserProfileMobileDto? UserProfile { get; set; } = null;
        public List<SpecialAdvertismentMobileDto> SpecialAdvertismentMobileDtos { get; set; } = new List<SpecialAdvertismentMobileDto>();
        public List<SitePropertyListingMobileDto> SiteProperties { get; set; } = new List<SitePropertyListingMobileDto>();
        public List<SitePropertyListingMobileDto> HighlyRatedProperty { get; set; } = new List<SitePropertyListingMobileDto>();
       // public List<HotelMobileDto> HotelOfTheweekDto { get; set; } = new List<GovernorateMobileDto>();
        public List<GovernorateMobileDto> GovernorateMobileDto { get; set; } = new List<GovernorateMobileDto>();

        public List<UserProfileMobileDto> HotelsOfTheWeek { get; set; } = new();
        public OnlyForYouSectionMobileDto onlyForYouSectionMobileDto { get; set; } = new OnlyForYouSectionMobileDto();

    }
}
