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
using AhlanFeekum.Reservations;
using AhlanFeekum.Shared;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.SpecialAdvertisments;
using AhlanFeekum.Statuses;
using AhlanFeekum.UserProfiles;
using AhlanFeekum.AhlanfeekumTerms;
using AhlanFeekum.Tickets;
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
        CreateMap<PropertyEvaluationWithNavigationProperties, PropertyEvaluationMobileDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PropertyEvaluation.Id))
            .ForMember(dest => dest.Cleanliness, opt => opt.MapFrom(src => src.PropertyEvaluation.Cleanliness))
            .ForMember(dest => dest.PriceAndValue, opt => opt.MapFrom(src => src.PropertyEvaluation.PriceAndValue))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.PropertyEvaluation.Location))
            .ForMember(dest => dest.Accuracy, opt => opt.MapFrom(src => src.PropertyEvaluation.Accuracy))
            .ForMember(dest => dest.Attitude, opt => opt.MapFrom(src => src.PropertyEvaluation.Attitude))
            .ForMember(dest => dest.RatingComment, opt => opt.MapFrom(src => src.PropertyEvaluation.RatingComment))
            .ForMember(dest => dest.UserProfileId, opt => opt.MapFrom(src => src.UserProfile.Id))
            .ForMember(dest => dest.UserProfileName, opt => opt.MapFrom(src => src.UserProfile.Name));

        CreateMap<PropertyMedia, PropertyMediaDto>();
        CreateMap<PropertyMedia, PropertyMediaExcelDto>();
        CreateMap<PropertyMediaWithNavigationProperties, PropertyMediaWithNavigationPropertiesDto>();
        CreateMap<PropertyMedia, PropertyMediaMobileDto>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null ? $"{MimeTypes.MimeTypeMap.GetAttachmentPath()}/propertyMedias/{src.Image}" : null));



        CreateMap<MobileResponse, MobileResponseDto>();


        CreateMap<Governorate, GovernorateDto>();
        CreateMap<Governorate, GovernorateMobileDto>()
            .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.IconId != null ? $"{MimeTypes.MimeTypeMap.GetAttachmentPath()}/governorate-file/{src.IconId.ToString("N")}{src.iconExtension}" : null));
        CreateMap<Governorate, GovernorateExcelDto>();

        CreateMap<Governorate, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Title));

        CreateMap<SpecialAdvertisment, SpecialAdvertismentDto>();
        CreateMap<SpecialAdvertisment, SpecialAdvertismentExcelDto>();
        CreateMap<SpecialAdvertismentWithNavigationProperties, SpecialAdvertismentWithNavigationPropertiesDto>();
        CreateMap<SpecialAdvertismentWithNavigationProperties, SpecialAdvertismentMobileDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SpecialAdvertisment.Id))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.SpecialAdvertisment.ImageId != null ? $"{MimeTypes.MimeTypeMap.GetAttachmentPath()}/specialadvertisment-file/{src.SpecialAdvertisment.ImageId.ToString("N")}{src.SpecialAdvertisment.ImageExtension}" : null))
            .ForMember(dest => dest.SitePropertyId, opt => opt.MapFrom(src => src.SpecialAdvertisment.Id))
            .ForMember(dest => dest.SitePropertyTitle, opt => opt.MapFrom(src => src.SiteProperty.PropertyTitle));


        CreateMap<OnlyForYouSection, OnlyForYouSectionDto>();
        CreateMap<OnlyForYouSection, OnlyForYouSectionMobileDto>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstPhoto, opt => opt.MapFrom(src => src.FirstPhotoId != null ? $"{MimeTypes.MimeTypeMap.GetAttachmentPath()}/onlyforyousection-file/{src.FirstPhotoId.ToString("N")}{src.FirstPhotoExtension}" : null))
            .ForMember(dest => dest.SecondPhoto, opt => opt.MapFrom(src => src.SecondPhotoId != null ? $"{MimeTypes.MimeTypeMap.GetAttachmentPath()}/onlyforyousection-file/{src.SecondPhotoId.ToString("N")}{src.SecondPhotoExtension}" : null))
            .ForMember(dest => dest.ThirdPhoto, opt => opt.MapFrom(src => src.ThirdPhotoId != null ? $"{MimeTypes.MimeTypeMap.GetAttachmentPath()}/onlyforyousection-file/{src.ThirdPhotoId.ToString("N")}{src.ThirdPhotoExtension}" : null));
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


        CreateMap<Status, StatusDto>();
        CreateMap<Status, StatusExcelDto>();

        CreateMap<StatusDto, StatusUpdateDto>();
        CreateMap<Status, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));


        CreateMap<Reservation, ReservationDto>();
        CreateMap<Reservation, ReservationExcelDto>();
        CreateMap<ReservationWithNavigationProperties, ReservationWithNavigationPropertiesDto>();
        CreateMap<ReservationWithNavigationProperties, ReservationMobileDto>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Reservation.Id))
              .ForMember(dest => dest.FromeDate, opt => opt.MapFrom(src => src.Reservation.FromeDate))
              .ForMember(dest => dest.ToDate, opt => opt.MapFrom(src => src.Reservation.ToDate))
              .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.Reservation.CheckInDate))
              .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.Reservation.CheckOutDate))
              .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Reservation.Price))
              .ForMember(dest => dest.NumberOfGuest, opt => opt.MapFrom(src => src.Reservation.NumberOfGuest))
              .ForMember(dest => dest.ReservationStatus, opt => opt.MapFrom(src => src.Reservation.ReservationStatus))
              .ForMember(dest => dest.ReservationStatusAsString, opt => opt.MapFrom(src => src.Reservation.ReservationStatus.ToString()))
              .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Reservation.Notes))
              .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Reservation.Discount))
              .ForMember(dest => dest.UserProfileId, opt => opt.MapFrom(src => src.Reservation.UserProfileId))
              .ForMember(dest => dest.UserProfileName, opt => opt.MapFrom(src => src.UserProfile.Name))
              .ForMember(dest => dest.UserProfilePhoto, opt => opt.MapFrom(src => !src.UserProfile.ProfilePhoto .IsNullOrEmpty() ? $"{AhlanFeekum.MimeTypes.MimeTypeMap.GetAttachmentPath()}/UserProfileImages/{src.UserProfile.ProfilePhoto}" : ""))
              .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.PropertyOwner!= null  ? src.PropertyOwner.Id : (Guid?)null))
              .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.PropertyOwner != null ? src.PropertyOwner.Name : ""))
              .ForMember(dest => dest.OwnerProfilePhoto, opt => opt.MapFrom(src => src.PropertyOwner != null ?  $"{AhlanFeekum.MimeTypes.MimeTypeMap.GetAttachmentPath()}/UserProfileImages/{src.PropertyOwner.ProfilePhoto}" : ""))
              .ForMember(dest => dest.PropertyId, opt => opt.MapFrom(src => src.Reservation.SitePropertyId))
              .ForMember(dest => dest.PropertyTitle, opt => opt.MapFrom(src => src.SiteProperty.PropertyTitle))
              .ForMember(dest => dest.PropertyArea, opt => opt.MapFrom(src => src.SiteProperty.Area))
              .ForMember(dest => dest.PropertyMainImage, opt => opt.MapFrom(src => src.PropertyMedia != null? $"{MimeTypes.MimeTypeMap.GetAttachmentPath()}/propertyMedias/{src.PropertyMedia.Image}" : ""));
          //  .ForMember(dest => dest.FirstPhoto, opt => opt.MapFrom(src => src.FirstPhotoId != null ? $"{MimeTypes.MimeTypeMap.GetAttachmentPath()}/onlyforyousection-file/{src.FirstPhotoId.ToString("N")}{src.FirstPhotoExtension}" : null))


        CreateMap<ReservationDto, ReservationUpdateDto>();


        CreateMap<Ticket, TicketDto>();
        CreateMap<Ticket, TicketExcelDto>();
        CreateMap<TicketWithNavigationProperties, TicketWithNavigationPropertiesDto>();
        CreateMap<TicketDto, TicketUpdateDto>();

        CreateMap<AhlanfeekumTerm, AhlanfeekumTermDto>();
        CreateMap<AhlanfeekumTerm, AhlanfeekumTermExcelDto>();
        CreateMap<AhlanfeekumTermDto, AhlanfeekumTermUpdateDto>();

        CreateMap<AhlanfeekumTerm, AhlanfeekumTermMobileDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.TermsIcon, opt => opt.MapFrom(src => $"{MimeTypes.MimeTypeMap.GetAttachmentPath()}/ahlanfeekumterm-file/{src.TermsIconId.ToString("N")}{src.TermsIconExtension}"))
           .ForMember(dest => dest.WhoAreWeIcon, opt => opt.MapFrom(src => $"{MimeTypes.MimeTypeMap.GetAttachmentPath()}/ahlanfeekumterm-file/{src.WhoAreWeIconId.ToString("N")}{src.WhoAreWeIconExtension}"));
    }
}
