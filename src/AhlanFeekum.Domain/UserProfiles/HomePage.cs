using AhlanFeekum.Governorates;
using AhlanFeekum.OnlyForYouSections;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.SpecialAdvertisments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhlanFeekum.UserProfiles
{
    public class HomePage
    {
        public List<SpecialAdvertismentWithNavigationProperties> SpecialAdvertisments { get; set; }
        public List<Governorate> Governorates { get; set; }
        public List<SitePropertyWithDetails> SitePropertyWithDetails { get; set; }
        public OnlyForYouSection OnlyForYouSection { get; set; }


    }
}
