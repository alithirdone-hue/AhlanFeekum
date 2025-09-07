using Volo.Abp.Application.Dtos;
using System;
using System.Collections.Generic;

namespace AhlanFeekum.SiteProperties
{
    public  class GetSitePropertiesMobileInput : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public Guid? PropertyTypeId { get; set; }
        public DateOnly? CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public int? PricePerNightMin { get; set; }
        public int? PricePerNightMax { get; set; }
        public string? Address { get; set; }
        public int? BedroomsMin { get; set; }
        public int? BedroomsMax { get; set; }
        public int? BathroomsMin { get; set; }
        public int? BathroomsMax { get; set; }

        public int? NumberOfBedMin { get; set; }
        public int? NumberOfBedMax { get; set; }

        public Guid? GovernorateId { get; set; }
        public List<Guid?> PropertyFeatureIds { get; set; }



        public string? PropertyTitle { get; set; }
        public string? HotelName { get; set; }



        public int? FloorMin { get; set; }
        public int? FloorMax { get; set; }
        public int? MaximumNumberOfGuestMin { get; set; }
        public int? MaximumNumberOfGuestMax { get; set; }
        public int? LivingroomsMin { get; set; }
        public int? LivingroomsMax { get; set; }
        public string? PropertyDescription { get; set; }
        public string? HourseRules { get; set; }
        public string? ImportantInformation { get; set; }
       
        public string? StreetAndBuildingNumber { get; set; }
        public string? LandMark { get; set; }

        public bool? IsActive { get; set; }
        public GetSitePropertiesMobileInput()
        {

        }
    }
}