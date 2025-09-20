using AhlanFeekum.PropertyFeatures;
using AhlanFeekum.PropertyMedias;
using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.SiteProperties
{
    public  class SitePropertyListingMobileDto
    {
        public Guid Id { get; set; }
        public string PropertyTitle { get; set; } = null!;
        public string? HotelName { get; set; }        
        public string? Address { get; set; }
        public string? StreetAndBuildingNumber { get; set; }
        public string? LandMark { get; set; }
        public double? AverageRating { get; set; } = null;
        public int PricePerNight { get; set; }
        public bool IsActive { get; set; }
        public bool IsFavorite { get; set; } = false;
        //public Guid PropertyTypeId { get; set; }
        //public string PropertyTypeName { get; set; }
        //public Guid GovernorateId { get; set; }
        //public string GovernorateName { get; set; }
        public  double Area { get; set; }
        public string? MainImage { get; set; }

    }
}