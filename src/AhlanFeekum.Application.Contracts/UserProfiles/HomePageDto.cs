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
        public List<SpecialAdvertismentMobileDto> SpecialAdvertismentMobileDtos { get; set; } = new List<SpecialAdvertismentMobileDto>();
        public List<GovernorateMobileDto> GovernorateMobileDto { get; set; } = new List<GovernorateMobileDto>();
        public List<SitePropertyMobileDto> SitePropertyMobileDtos { get; set; } = new List<SitePropertyMobileDto>();
        public OnlyForYouSectionMobileDto onlyForYouSectionMobileDto { get; set; } = new OnlyForYouSectionMobileDto();

    }
}
