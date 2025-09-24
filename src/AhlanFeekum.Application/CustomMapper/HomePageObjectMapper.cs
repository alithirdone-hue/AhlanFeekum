using AhlanFeekum.Governorates;
using AhlanFeekum.MimeTypes;
using AhlanFeekum.OnlyForYouSections;
using AhlanFeekum.PropertyFeatures;
using AhlanFeekum.PropertyMedias;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.SpecialAdvertisments;
using AhlanFeekum.UserProfiles;
using System.Collections.Generic;
using System.Globalization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.ObjectMapping;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace SIBF.CustomMapper
{
    public class HomePageObjectMapper : IObjectMapper<HomePage, HomePageDto>,
        IObjectMapper<List<HomePage>, List<HomePageDto>>,
        ITransientDependency
    {
        private readonly IObjectMapper _objectMapper;

        public HomePageObjectMapper(

           IObjectMapper objectMapper
            )
        {
            _objectMapper = objectMapper;
        }

        public List<HomePageDto> Map(List<HomePage> source)
        {

            var output = new List<HomePageDto>();
            foreach (var item in source)
            {
                output.Add(Map(item));
            }

            return output;
        }

        public List<HomePageDto> Map(List<HomePage> source, List<HomePageDto> destination)
        {
            return Map(source);
        }

        public HomePageDto Map(HomePage source)
        {

            HomePageDto HomePageFront = new HomePageDto();
            HomePageFront.UserProfile = source.UserProfile != null ?_objectMapper.Map<UserProfile, UserProfileMobileDto>(source.UserProfile)  : null;
            HomePageFront.SpecialAdvertismentMobileDtos = _objectMapper.Map<List<SpecialAdvertismentWithNavigationProperties>, List<SpecialAdvertismentMobileDto>>(source.SpecialAdvertisments);
            HomePageFront.SiteProperties = _objectMapper.Map<List<SitePropertyWithDetails>, List<SitePropertyListingMobileDto>>(source.SiteProperties);
            HomePageFront.HighlyRatedProperty = _objectMapper.Map<List<SitePropertyWithDetails>, List<SitePropertyListingMobileDto>>(source.HighlyRated);
            HomePageFront.GovernorateMobileDto = _objectMapper.Map<List<Governorate>, List<GovernorateMobileDto>>(source.Governorates);
            HomePageFront.onlyForYouSectionMobileDto = _objectMapper.Map<OnlyForYouSection, OnlyForYouSectionMobileDto> (source.OnlyForYouSection);
                  
            return HomePageFront;
        }

        public HomePageDto Map(HomePage source, HomePageDto destination)
        {
            return Map(source);
        }
    }
}


