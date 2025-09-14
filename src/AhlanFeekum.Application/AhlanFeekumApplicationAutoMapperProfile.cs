using AhlanFeekum.FavoriteProperties;
using AhlanFeekum.Governorates;
using AhlanFeekum.MobileResponses;
using AhlanFeekum.OnlyForYouSections;
using AhlanFeekum.PersonEvaluations;
using AhlanFeekum.PropertyCalendars;
using AhlanFeekum.PropertyEvaluations;
using AhlanFeekum.PropertyFeatures;
using AhlanFeekum.PropertyMedias;
using AhlanFeekum.PropertyTypes;
using AhlanFeekum.Shared;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.SpecialAdvertisments;
using AhlanFeekum.UserProfiles;
using AhlanFeekum.AppFileDescriptors;
using AhlanFeekum.Shared;
using AutoMapper;
using System;
using Volo.Abp.AutoMapper;
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
        CreateMap<PropertyFeature, PropertyFeatureMobileDto>()
            .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon != null ? $"{MimeTypes.MimeTypeMap.GetAttachmentPath()}/propertyFeatures/{src.Icon}" : null));


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
        CreateMap<PropertyMedia, PropertyMediaMobileDto>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null ? $"{MimeTypes.MimeTypeMap.GetAttachmentPath()}/propertyMedias/{src.Image}" : null));



        CreateMap<MobileResponse, MobileResponseDto>();


        CreateMap<Governorate, GovernorateDto>();
        CreateMap<Governorate, GovernorateMobileDto>();
        CreateMap<Governorate, GovernorateExcelDto>();

        CreateMap<Governorate, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Title));

        CreateMap<SpecialAdvertisment, SpecialAdvertismentDto>();
        CreateMap<SpecialAdvertisment, SpecialAdvertismentExcelDto>();
        CreateMap<SpecialAdvertismentWithNavigationProperties, SpecialAdvertismentWithNavigationPropertiesDto>();
        CreateMap<SpecialAdvertismentWithNavigationProperties, SpecialAdvertismentMobileDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SpecialAdvertisment.Id))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.SpecialAdvertisment.ImageId != null ? $"{MimeTypes.MimeTypeMap.GetAttachmentPath()}/specialadvertisment-file/{src.SpecialAdvertisment.ImageId}" : null))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.SpecialAdvertisment.IsActive))
            .ForMember(dest => dest.SitePropertyId, opt => opt.MapFrom(src => src.SpecialAdvertisment.Id))
            .ForMember(dest => dest.SitePropertyTitle, opt => opt.MapFrom(src => src.SiteProperty.PropertyTitle));


        CreateMap<OnlyForYouSection, OnlyForYouSectionDto>();
        CreateMap<OnlyForYouSection, OnlyForYouSectionExcelDto>();

        CreateMap<UserProfileDto, UserProfileUpdateDto>();

        CreateMap<PropertyFeatureDto, PropertyFeatureUpdateDto>();

        CreateMap<PropertyTypeDto, PropertyTypeUpdateDto>();

        CreateMap<SitePropertyDto, SitePropertyUpdateDto>().Ignore(x => x.PropertyFeatureIds);

        CreateMap<FavoritePropertyDto, FavoritePropertyUpdateDto>();

        CreateMap<PersonEvaluationDto, PersonEvaluationUpdateDto>();

        CreateMap<PropertyEvaluationDto, PropertyEvaluationUpdateDto>();

        CreateMap<PropertyMediaDto, PropertyMediaUpdateDto>();

        CreateMap<GovernorateDto, GovernorateUpdateDto>();

        CreateMap<SpecialAdvertismentDto, SpecialAdvertismentUpdateDto>();

        CreateMap<OnlyForYouSectionDto, OnlyForYouSectionUpdateDto>();


        CreateMap<PropertyCalendar, PropertyCalendarDto>();
        CreateMap<PropertyCalendar, PropertyCalendarExcelDto>();
        CreateMap<PropertyCalendarWithNavigationProperties, PropertyCalendarWithNavigationPropertiesDto>();
        CreateMap<PropertyCalendarDto, PropertyCalendarUpdateDto>();

        CreateMap<AppFileDescriptors.AppFileDescriptor, AppFileDescriptorDto>();

    }
}
