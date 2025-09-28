using AhlanFeekum.MimeTypes;
using AhlanFeekum.PropertyEvaluations;
using AhlanFeekum.PropertyFeatures;
using AhlanFeekum.PropertyMedias;
using AhlanFeekum.SiteProperties;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.ObjectMapping;
using static AhlanFeekum.Permissions.AhlanFeekumPermissions;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace SIBF.CustomMapper
{
    public class SitePropertyWithDetailsObjectMapper : IObjectMapper<SitePropertyWithDetails, SitePropertyWithDetailsMobileDto>,
        IObjectMapper<List<SitePropertyWithDetails>, List<SitePropertyWithDetailsMobileDto>>,
        ITransientDependency
    {
        private readonly IObjectMapper _objectMapper;

        public SitePropertyWithDetailsObjectMapper(

           IObjectMapper objectMapper
            )
        {
            _objectMapper = objectMapper;
        }

        public List<SitePropertyWithDetailsMobileDto> Map(List<SitePropertyWithDetails> source)
        {

            List<SitePropertyWithDetailsMobileDto> output = new List<SitePropertyWithDetailsMobileDto>();
            foreach (var item in source)
            {
                output.Add(Map(item));
            }

            return output;
        }

        public List<SitePropertyWithDetailsMobileDto> Map(List<SitePropertyWithDetails> source, List<SitePropertyWithDetailsMobileDto> destination)
        {
            return Map(source);
        }

        public SitePropertyWithDetailsMobileDto Map(SitePropertyWithDetails source)
        {

            SitePropertyWithDetailsMobileDto SitePropertyWithDetailsFront = new SitePropertyWithDetailsMobileDto();
            SitePropertyWithDetailsFront.Id = source.SiteProperty.Id;
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
            SitePropertyWithDetailsFront.Latitude = source.SiteProperty.Latitude;
            SitePropertyWithDetailsFront.Longitude = source.SiteProperty.Longitude;
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
                SitePropertyWithDetailsFront.MainImage = !source.Medias.IsNullOrEmpty() ?  $"{MimeTypeMap.GetAttachmentPath()}/propertyMedias/{source.Medias[0].Image}" : null;
                SitePropertyWithDetailsFront.PropertyMediaMobileDto = _objectMapper.Map<List<PropertyMedia>, List<PropertyMediaMobileDto>>(source.Medias);
            }
            SitePropertyWithDetailsFront.IsActive = source.SiteProperty.IsActive;
            SitePropertyWithDetailsFront.OwnerId = source.SiteProperty.OwnerId;
            SitePropertyWithDetailsFront.OwnerName = source.Owner.Name;
            SitePropertyWithDetailsFront.StatusId = source.SiteProperty.StatusId;
            SitePropertyWithDetailsFront.StatusName = source.Status.Name;
            SitePropertyWithDetailsFront.Area = source.SiteProperty.Area;

            if(!source.PropertyEvaluationWithNavigationProperties.IsNullOrEmpty())
                SitePropertyWithDetailsFront.PropertyEvaluationMobileDtos = _objectMapper.Map<List<PropertyEvaluationWithNavigationProperties>, List<PropertyEvaluationMobileDto>>(source.PropertyEvaluationWithNavigationProperties);

            if (!source.PropertyEvaluationWithNavigationProperties.IsNullOrEmpty())
            {
                SitePropertyWithDetailsFront.AverageCleanliness = source.PropertyEvaluationWithNavigationProperties.Average(x => (double?)x.PropertyEvaluation.Cleanliness);
                SitePropertyWithDetailsFront.AveragePriceAndValue = source.PropertyEvaluationWithNavigationProperties.Average(x => (double?)x.PropertyEvaluation.PriceAndValue);
                SitePropertyWithDetailsFront.AverageLocation = source.PropertyEvaluationWithNavigationProperties.Average(x => (double?)x.PropertyEvaluation.Location);
                SitePropertyWithDetailsFront.AverageAccuracy = source.PropertyEvaluationWithNavigationProperties.Average(x => (double?)x.PropertyEvaluation.Accuracy);
                SitePropertyWithDetailsFront.AverageAttitude = source.PropertyEvaluationWithNavigationProperties.Average(x => (double?)x.PropertyEvaluation.Attitude);
                SitePropertyWithDetailsFront.AverageRating = (SitePropertyWithDetailsFront.AverageCleanliness + SitePropertyWithDetailsFront.AveragePriceAndValue + SitePropertyWithDetailsFront.AverageLocation + SitePropertyWithDetailsFront.AverageAccuracy
                                                                + SitePropertyWithDetailsFront.AverageAttitude) / 5;
            }
            else
            {
                SitePropertyWithDetailsFront.AverageCleanliness = null;
                SitePropertyWithDetailsFront.AveragePriceAndValue = null;
                SitePropertyWithDetailsFront.AverageLocation = null;
                SitePropertyWithDetailsFront.AverageAccuracy = null;
                SitePropertyWithDetailsFront.AverageAttitude = null;
                SitePropertyWithDetailsFront.AverageRating = null;
            }

            return SitePropertyWithDetailsFront;
        }

        public SitePropertyWithDetailsMobileDto Map(SitePropertyWithDetails source, SitePropertyWithDetailsMobileDto destination)
        {
            return Map(source);
        }




    }
}


