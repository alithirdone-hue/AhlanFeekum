using AhlanFeekum.PropertyFeatures;
using AhlanFeekum.PropertyMedias;
using AhlanFeekum.SiteProperties;
using System.Collections.Generic;
using System.Globalization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.ObjectMapping;
using AhlanFeekum.MimeTypes;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace SIBF.CustomMapper
{
    public class SitePropertyObjectMapper : IObjectMapper<SitePropertyWithDetails, SitePropertyMobileDto>,
        IObjectMapper<List<SitePropertyWithDetails>, List<SitePropertyMobileDto>>,
        ITransientDependency
    {
        private readonly IObjectMapper _objectMapper;

        public SitePropertyObjectMapper(

           IObjectMapper objectMapper
            )
        {
            _objectMapper = objectMapper;
        }

        public List<SitePropertyMobileDto> Map(List<SitePropertyWithDetails> source)
        {

            List<SitePropertyMobileDto> output = new List<SitePropertyMobileDto>();
            foreach (var item in source)
            {
                output.Add(Map(item));
            }

            return output;
        }

        public List<SitePropertyMobileDto> Map(List<SitePropertyWithDetails> source, List<SitePropertyMobileDto> destination)
        {
            return Map(source);
        }

        public SitePropertyMobileDto Map(SitePropertyWithDetails source)
        {

            SitePropertyMobileDto SitePropertyWithDetailsFront = new SitePropertyMobileDto();
            SitePropertyWithDetailsFront.PropertyTitle = source.SiteProperty.PropertyTitle;
            SitePropertyWithDetailsFront.HotelName = source.SiteProperty.HotelName;
            SitePropertyWithDetailsFront.Bedrooms = source.SiteProperty.Bedrooms;
            SitePropertyWithDetailsFront.Bathrooms = source.SiteProperty.Bathrooms;
            SitePropertyWithDetailsFront.NumberOfBed = source.SiteProperty.NumberOfBed;
            SitePropertyWithDetailsFront.Floor = source.SiteProperty.Floor;
            SitePropertyWithDetailsFront.MaximumNumberOfGuest = source.SiteProperty.MaximumNumberOfGuest;
            SitePropertyWithDetailsFront.Livingrooms = source.SiteProperty.Livingrooms;
            SitePropertyWithDetailsFront.PropertyDescription = source.SiteProperty.PropertyDescription;
            SitePropertyWithDetailsFront.HourseRules = source.SiteProperty.HourseRules;
            SitePropertyWithDetailsFront.ImportantInformation = source.SiteProperty.ImportantInformation;
            SitePropertyWithDetailsFront.StreetAndBuildingNumber = source.SiteProperty.StreetAndBuildingNumber;
            SitePropertyWithDetailsFront.LandMark = source.SiteProperty.LandMark;
            SitePropertyWithDetailsFront.PricePerNight = source.SiteProperty.PricePerNight;
            SitePropertyWithDetailsFront.IsActive = source.SiteProperty.IsActive;
            SitePropertyWithDetailsFront.IsFavorite = source.IsFavorite;
            SitePropertyWithDetailsFront.PropertyTypeId = source.PropertyType.Id;
            SitePropertyWithDetailsFront.PropertyTypeName = source.PropertyType.Title;
            SitePropertyWithDetailsFront.GovernorateId = source.Governorate.Id;
            SitePropertyWithDetailsFront.GovernorateName = source.Governorate.Title;
            SitePropertyWithDetailsFront.PropertyFeatureMobileDtos = _objectMapper.Map<List<PropertyFeature>, List<PropertyFeatureMobileDto>>(source.PropertyFeatures);
            if (!source.Medias.IsNullOrEmpty())
            {
                SitePropertyWithDetailsFront.MainImage = $"{MimeTypeMap.GetAttachmentPath()}/propertyMedias/{source.Medias[0].Image}";
                SitePropertyWithDetailsFront.PropertyMediaMobileDto = _objectMapper.Map<List<PropertyMedia>, List<PropertyMediaMobileDto>>(source.Medias);
            }
            SitePropertyWithDetailsFront.IsActive = source.SiteProperty.IsActive;
            SitePropertyWithDetailsFront.IsActive = source.SiteProperty.IsActive;
            SitePropertyWithDetailsFront.IsActive = source.SiteProperty.IsActive;

       
            return SitePropertyWithDetailsFront;
        }

        public SitePropertyMobileDto Map(SitePropertyWithDetails source, SitePropertyMobileDto destination)
        {
            return Map(source);
        }




    }
}


