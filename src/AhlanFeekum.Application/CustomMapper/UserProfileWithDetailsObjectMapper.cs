using AhlanFeekum.PropertyFeatures;
using AhlanFeekum.PropertyMedias;
using AhlanFeekum.SiteProperties;
using System.Collections.Generic;
using System.Globalization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using AhlanFeekum.MimeTypes;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;
using AhlanFeekum.UserProfiles;

namespace SIBF.CustomMapper
{
    public class UserProfileWithDetailsObjectMapper : IObjectMapper<UserProfileWithDetails, UserProfileWithDetailsMobileDto>,
        IObjectMapper<List<UserProfileWithDetails>, List<UserProfileWithDetailsMobileDto>>,
        ITransientDependency
    {
        private readonly IObjectMapper _objectMapper;

        public UserProfileWithDetailsObjectMapper(

           IObjectMapper objectMapper
            )
        {
            _objectMapper = objectMapper;
        }

        public List<UserProfileWithDetailsMobileDto> Map(List<UserProfileWithDetails> source)
        {

            List<UserProfileWithDetailsMobileDto> output = new List<UserProfileWithDetailsMobileDto>();
            foreach (var item in source)
            {
                output.Add(Map(item));
            }

            return output;
        }

        public List<UserProfileWithDetailsMobileDto> Map(List<UserProfileWithDetails> source, List<UserProfileWithDetailsMobileDto> destination)
        {
            return Map(source);
        }

        public UserProfileWithDetailsMobileDto Map(UserProfileWithDetails source)
        {

            UserProfileWithDetailsMobileDto UserProfileWithDetailsFront = new UserProfileWithDetailsMobileDto();
            UserProfileWithDetailsFront.Id = source.UserProfile.Id;
            UserProfileWithDetailsFront.Name = source.UserProfile.Name;
            UserProfileWithDetailsFront.Email = source.UserProfile.Email;
            UserProfileWithDetailsFront.PhoneNumber = source.UserProfile.PhoneNumber;
            UserProfileWithDetailsFront.Latitude = source.UserProfile.Latitude;
            UserProfileWithDetailsFront.Longitude = source.UserProfile.Longitude;
            UserProfileWithDetailsFront.Address = source.UserProfile.Address;
            UserProfileWithDetailsFront.IsSuperHost = source.UserProfile.IsSuperHost;
            if (source.UserProfile.ProfilePhoto != null)
            {
                UserProfileWithDetailsFront.ProfilePhoto = $"{AhlanFeekum.MimeTypes.MimeTypeMap.GetAttachmentPath()}/UserProfileImages/{source.UserProfile.ProfilePhoto}";
            }
            if(!source.MyProperties.IsNullOrEmpty())
                UserProfileWithDetailsFront.MyProperties = _objectMapper.Map<List<SitePropertyWithDetails>, List<SitePropertyListingMobileDto>>(source.MyProperties);
            if(!source.FavoriteProperties.IsNullOrEmpty())
                UserProfileWithDetailsFront.FavoriteProperties = _objectMapper.Map<List<SitePropertyWithDetails>, List<SitePropertyListingMobileDto>>(source.FavoriteProperties);
         
           
            return UserProfileWithDetailsFront;
        }

        public UserProfileWithDetailsMobileDto Map(UserProfileWithDetails source, UserProfileWithDetailsMobileDto destination)
        {
            return Map(source);
        }




    }
}


