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
    public class SitePropertyListingObjectMapper :  IObjectMapper<SitePropertyWithDetails, SitePropertyListingMobileDto>,
        IObjectMapper<List<SitePropertyWithDetails>, List<SitePropertyListingMobileDto>>,
        ITransientDependency
    {
        private readonly IObjectMapper _objectMapper;

        public SitePropertyListingObjectMapper(

           IObjectMapper objectMapper
            )
        {
            _objectMapper = objectMapper;
        }


        public List<SitePropertyListingMobileDto> Map(List<SitePropertyWithDetails> source)
        {

            var output = new List<SitePropertyListingMobileDto>();
            foreach (var item in source)
            {
                output.Add(Map(item));
            }

            return output;
        }

        public List<SitePropertyListingMobileDto> Map(List<SitePropertyWithDetails> source, List<SitePropertyListingMobileDto> destination)
        {
            return Map(source);
        }

        public SitePropertyListingMobileDto Map(SitePropertyWithDetails source)
        {

            SitePropertyListingMobileDto SitePropertyWithDetailsFront = new SitePropertyListingMobileDto();
            SitePropertyWithDetailsFront.Id = source.SiteProperty.Id;
            SitePropertyWithDetailsFront.PropertyTitle = source.SiteProperty.PropertyTitle;
            SitePropertyWithDetailsFront.HotelName = source.SiteProperty.HotelName;
           
            SitePropertyWithDetailsFront.StreetAndBuildingNumber = source.SiteProperty.StreetAndBuildingNumber;
            SitePropertyWithDetailsFront.LandMark = source.SiteProperty.LandMark;
            SitePropertyWithDetailsFront.PricePerNight = source.SiteProperty.PricePerNight;
            SitePropertyWithDetailsFront.AverageRating = source.AverageRating;
            SitePropertyWithDetailsFront.Area = source.SiteProperty.Area;
            SitePropertyWithDetailsFront.IsActive = source.SiteProperty.IsActive;
            SitePropertyWithDetailsFront.IsFavorite = source.IsFavorite;
            
            SitePropertyWithDetailsFront.MainImage = source.MainImage != null ? $"{MimeTypeMap.GetAttachmentPath()}/propertyMedias/{source.MainImage.Image}" : null;
            //if(source.PropertyType != null)
            //{
            //    SitePropertyWithDetailsFront.PropertyTypeId = source.PropertyType.Id;
            //    SitePropertyWithDetailsFront.PropertyTypeName = source.PropertyType.Title;
            //}
            //if (source.Governorate != null)
            //{
            //    SitePropertyWithDetailsFront.PropertyTypeId = source.PropertyType.Id;
            //    SitePropertyWithDetailsFront.PropertyTypeName = source.PropertyType.Title;
            //}
            return SitePropertyWithDetailsFront;
        }

        public SitePropertyListingMobileDto Map(SitePropertyWithDetails source, SitePropertyListingMobileDto destination)
        {
            return Map(source);
        }
    }
}


