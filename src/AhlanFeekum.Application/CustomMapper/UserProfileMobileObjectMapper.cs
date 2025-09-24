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
    public class UserProfileMobileObjectMapper : IObjectMapper<UserProfile, UserProfileMobileDto>,
        IObjectMapper<List<UserProfile>, List<UserProfileMobileDto>>,
        ITransientDependency
    {

        public UserProfileMobileObjectMapper(
            )
        {
        }

        public List<UserProfileMobileDto> Map(List<UserProfile> source)
        {

            List<UserProfileMobileDto> output = new List<UserProfileMobileDto>();
            foreach (var item in source)
            {
                output.Add(Map(item));
            }

            return output;
        }

        public List<UserProfileMobileDto> Map(List<UserProfile> source, List<UserProfileMobileDto> destination)
        {
            return Map(source);
        }

        public UserProfileMobileDto Map(UserProfile source)
        {
            UserProfileMobileDto UserProfileWithDetailsFront = new UserProfileMobileDto();
            UserProfileWithDetailsFront.Id = source.Id;
            UserProfileWithDetailsFront.Name = source.Name;
            UserProfileWithDetailsFront.Email = source.Email;
            if (source.ProfilePhoto != null)
            {
                UserProfileWithDetailsFront.ProfilePhoto = $"{AhlanFeekum.MimeTypes.MimeTypeMap.GetAttachmentPath()}/UserProfileImages/{source.ProfilePhoto}";
            }
                    
            return UserProfileWithDetailsFront;
        }

        public UserProfileMobileDto Map(UserProfile source, UserProfileMobileDto destination)
        {
            return Map(source);
        }




    }
}


