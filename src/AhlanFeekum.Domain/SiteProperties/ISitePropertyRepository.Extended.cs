using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AhlanFeekum.SiteProperties
{
    public partial interface ISitePropertyRepository
    {
        Task<SitePropertyWithDetails> GetSitePropertyWithDetailsAsync(Guid id, Guid? userId);

        Task<SitePropertyListWithDetails> GetListWithDetailsAsync(
        string? filterText = null,
        string? propertyTitle = null,
        string? hotelName = null,
        int? bedroomsMin = null,
        int? bedroomsMax = null,
        int? bathroomsMin = null,
        int? bathroomsMax = null,
        int? numberOfBedMin = null,
        int? numberOfBedMax = null,
        int? floorMin = null,
        int? floorMax = null,
        int? maximumNumberOfGuestMin = null,
        int? maximumNumberOfGuestMax = null,
        int? livingroomsMin = null,
        int? livingroomsMax = null,
        string? propertyDescription = null,
        string? hourseRules = null,
        string? importantInformation = null,
        string? address = null,
        string? streetAndBuildingNumber = null,
        string? landMark = null,
        int? pricePerNightMin = null,
        int? pricePerNightMax = null,
                    double? areaMin = null,
            double? areaMax = null,
            string? latitude = null,
            string? longitude = null,
        bool? isActive = null,
        Guid? propertyTypeId = null,
        Guid? governorateId = null,
        List<Guid> propertyFeatureId = null,
        DateOnly? checkInDateMin = null,
        DateOnly? checkOutDateMax = null,
        Guid? userId = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );
    }
}