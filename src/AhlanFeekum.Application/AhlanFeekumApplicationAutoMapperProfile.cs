using AhlanFeekum.FavoriteProperties;
using AhlanFeekum.MobileResponses;
using AhlanFeekum.PersonEvaluations;
using AhlanFeekum.PropertyEvaluations;
using AhlanFeekum.PropertyFeatures;
using AhlanFeekum.PropertyMedias;
using AhlanFeekum.PropertyTypes;
using AhlanFeekum.Shared;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.UserProfiles;
using AutoMapper;
using System;
using Volo.Abp.Identity;

namespace AhlanFeekum;

public class AhlanFeekumApplicationAutoMapperProfile : Profile
{
    public AhlanFeekumApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<UserProfile, UserProfileDto>();
        CreateMap<UserProfile, UserProfileExcelDto>();
        CreateMap<UserProfileWithNavigationProperties, UserProfileWithNavigationPropertiesDto>();
        CreateMap<IdentityRole, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));
        CreateMap<IdentityUser, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.UserName));

        CreateMap<PropertyFeature, PropertyFeatureDto>();
        CreateMap<PropertyFeature, PropertyFeatureExcelDto>();

        CreateMap<PropertyType, PropertyTypeDto>();
        CreateMap<PropertyType, PropertyTypeExcelDto>();

        CreateMap<SiteProperty, SitePropertyDto>();
        CreateMap<SiteProperty, SitePropertyExcelDto>();
        CreateMap<SitePropertyWithNavigationProperties, SitePropertyWithNavigationPropertiesDto>();
        CreateMap<PropertyType, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Title));
        CreateMap<PropertyFeature, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Title));

        CreateMap<FavoriteProperty, FavoritePropertyDto>();
        CreateMap<FavoriteProperty, FavoritePropertyExcelDto>();
        CreateMap<FavoritePropertyWithNavigationProperties, FavoritePropertyWithNavigationPropertiesDto>();
        CreateMap<UserProfile, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));
        CreateMap<SiteProperty, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.PropertyTitle));

        CreateMap<PersonEvaluation, PersonEvaluationDto>();
        CreateMap<PersonEvaluation, PersonEvaluationExcelDto>();
        CreateMap<PersonEvaluationWithNavigationProperties, PersonEvaluationWithNavigationPropertiesDto>();

        CreateMap<PropertyEvaluation, PropertyEvaluationDto>();
        CreateMap<PropertyEvaluation, PropertyEvaluationExcelDto>();
        CreateMap<PropertyEvaluationWithNavigationProperties, PropertyEvaluationWithNavigationPropertiesDto>();

        CreateMap<PropertyMedia, PropertyMediaDto>();
        CreateMap<PropertyMedia, PropertyMediaExcelDto>();
        CreateMap<PropertyMediaWithNavigationProperties, PropertyMediaWithNavigationPropertiesDto>();

        CreateMap<MobileResponse, MobileResponseDto>(); 
    }
}
