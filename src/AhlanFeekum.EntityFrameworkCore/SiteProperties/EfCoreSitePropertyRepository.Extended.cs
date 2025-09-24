using AhlanFeekum.EntityFrameworkCore;
using AhlanFeekum.Governorates;
using AhlanFeekum.PropertyEvaluations;
using AhlanFeekum.PropertyFeatures;
using AhlanFeekum.PropertyTypes;
using AhlanFeekum.Statuses;
using AhlanFeekum.UserProfiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace AhlanFeekum.SiteProperties
{
    public class EfCoreSitePropertyRepository : EfCoreSitePropertyRepositoryBase, ISitePropertyRepository
    {
        public EfCoreSitePropertyRepository(IDbContextProvider<AhlanFeekumDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<SitePropertyWithDetails> GetSitePropertyWithDetailsAsync(Guid id, Guid? userId)
        {
            var dbContext = await GetDbContextAsync();

            return
            (from siteProperty in (await GetDbSetAsync()).Where(b => b.Id == id).Include(x => x.PropertyFeatures)
             join propertyType in dbContext.PropertyTypes on siteProperty.PropertyTypeId equals propertyType.Id into propertyTypes
             from propertyType in propertyTypes.DefaultIfEmpty()
             join governorate in dbContext.Governorates on siteProperty.GovernorateId equals governorate.Id into governorates
             from governorate in governorates.DefaultIfEmpty()
             join owner in (await GetDbContextAsync()).Set<UserProfile>() on siteProperty.OwnerId equals owner.Id into userProfiles
             from owner in userProfiles.DefaultIfEmpty()
             join status in (await GetDbContextAsync()).Set<Status>() on siteProperty.StatusId equals status.Id into statuses
             from status in statuses.DefaultIfEmpty()
             select new SitePropertyWithDetails
             {
                 SiteProperty = siteProperty,
                 PropertyType = propertyType,
                 Governorate = governorate,
                 PropertyFeatures = (from sitePropertyPropertyFeature in siteProperty.PropertyFeatures
                                     join _propertyFeature in dbContext.Set<PropertyFeature>() on sitePropertyPropertyFeature.PropertyFeatureId equals _propertyFeature.Id
                                     select _propertyFeature).ToList(),
                 MainImage = dbContext.PropertyMedias.Where(pm => pm.SitePropertyId == siteProperty.Id).OrderBy(pm => pm.Order).FirstOrDefault(),
                 Medias = dbContext.PropertyMedias.Where(pm => pm.SitePropertyId == siteProperty.Id).OrderBy(pm => pm.Order).ToList(),
                 IsFavorite = userId == null ? false : dbContext.FavoriteProperties.Any(p => p.SitePropertyId == siteProperty.Id && p.UserProfileId == userId),
                 Owner = owner,
                 Status = status,
                 PropertyEvaluationWithNavigationProperties = (from propertyEvaluation in (dbContext.PropertyEvaluations) where propertyEvaluation.SitePropertyId == siteProperty.Id
                                                              join userProfile in (dbContext.UserProfiles) on propertyEvaluation.UserProfileId equals userProfile.Id into userProfiles
                                                              from userProfile in userProfiles.DefaultIfEmpty()
                                                              select new PropertyEvaluationWithNavigationProperties
                                                              {
                                                                  PropertyEvaluation = propertyEvaluation,
                                                                  UserProfile = userProfile,
                                                                  SiteProperty = null
                                                              }).ToList()
             }).FirstOrDefault();

            //return (await GetDbSetAsync()).Where(b => b.Id == id).Include(x => x.PropertyFeatures)
            //    .Select(siteProperty => new SitePropertyWithDetails
            //    {
            //        SiteProperty = siteProperty,
            //        PropertyType = dbContext.Set<PropertyType>().FirstOrDefault(c => c.Id == siteProperty.PropertyTypeId),
            //        Governorate = dbContext.Set<Governorate>().FirstOrDefault(c => c.Id == siteProperty.GovernorateId),
            //        PropertyFeatures = (from sitePropertyPropertyFeatures in siteProperty.PropertyFeatures
            //                            join _propertyFeature in dbContext.Set<PropertyFeature>() on sitePropertyPropertyFeatures.PropertyFeatureId equals _propertyFeature.Id
            //                            select _propertyFeature).ToList()
            //    }).FirstOrDefault();
        }

        public virtual async Task<SitePropertyListWithDetails> GetListWithDetailsAsync(
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
     bool? isActive = null,
     Guid? propertyTypeId = null,
     Guid? governorateId = null,
     List<Guid> propertyFeatureIds = null,
     DateOnly? checkInDateMin = null,
     DateOnly? checkInDateMax = null,
     Guid? userId = null,
     string? sorting = null,
     int maxResultCount = int.MaxValue,
     int skipCount = 0,
     CancellationToken cancellationToken = default)
        {
            SitePropertyListWithDetails sitePropertyListWithDetails = new SitePropertyListWithDetails();
            var query = await GetQueryForDetailssAsync(userId);
            query = ApplyFilterWithDetails(query, filterText, propertyTitle, hotelName, bedroomsMin, bedroomsMax, bathroomsMin, bathroomsMax, numberOfBedMin, numberOfBedMax, floorMin, floorMax, maximumNumberOfGuestMin, maximumNumberOfGuestMax, livingroomsMin, livingroomsMax, propertyDescription, hourseRules, importantInformation, address, streetAndBuildingNumber, landMark, pricePerNightMin, pricePerNightMax, isActive, propertyTypeId, governorateId, propertyFeatureIds, checkInDateMin, checkInDateMax);
            sitePropertyListWithDetails.TotalCount = await query.LongCountAsync(GetCancellationToken(cancellationToken));
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? SitePropertyConsts.GetDefaultSorting(true) : sorting);
            sitePropertyListWithDetails.SitePropertyWithDetails =  await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);

            return sitePropertyListWithDetails;
        }
        protected virtual async Task<IQueryable<SitePropertyWithDetails>> GetQueryForDetailssAsync(Guid? userId)
        {
            var dbContext = await GetDbContextAsync();
            return from siteProperty in (await GetDbSetAsync())
                   join propertyType in (await GetDbContextAsync()).Set<PropertyType>() on siteProperty.PropertyTypeId equals propertyType.Id into propertyTypes
                   from propertyType in propertyTypes.DefaultIfEmpty()
                   join governorate in (await GetDbContextAsync()).Set<Governorate>() on siteProperty.GovernorateId equals governorate.Id into governorates
                   from governorate in governorates.DefaultIfEmpty()
                   
                   select new SitePropertyWithDetails
                   {
                       SiteProperty = siteProperty,
                       PropertyType = propertyType,
                       Governorate = governorate,
                       PropertyFeatures = new List<PropertyFeature>(),
                       MainImage = dbContext.PropertyMedias.Where(pm => pm.SitePropertyId == siteProperty.Id).OrderBy(pm => pm.Order).FirstOrDefault(),
                       Medias = new List<PropertyMedias.PropertyMedia>(),
                       IsFavorite = userId == null ? false : dbContext.FavoriteProperties.Any(p => p.SitePropertyId == siteProperty.Id && p.UserProfileId == userId),
                       AverageRating = dbContext.PropertyEvaluations.Where(p => p.SitePropertyId == siteProperty.Id).Average(e => (e.Cleanliness + e.PriceAndValue + e.Location + e.Accuracy + e.Attitude) / 5.0)

                   };
        }
        protected virtual IQueryable<SitePropertyWithDetails> ApplyFilterWithDetails(
             IQueryable<SitePropertyWithDetails> query,
             string? filterText,
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
             bool? isActive = null,
             Guid? propertyTypeId = null,
             Guid? governorateId = null,
             List<Guid> propertyFeatureIds = null,
             DateOnly? checkInDateMin = null,
             DateOnly? checkInDateMax = null,
             Guid? userId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.SiteProperty.PropertyTitle!.Contains(filterText!) || e.SiteProperty.HotelName!.Contains(filterText!) || e.SiteProperty.PropertyDescription!.Contains(filterText!) || e.SiteProperty.HourseRules!.Contains(filterText!) || e.SiteProperty.ImportantInformation!.Contains(filterText!) || e.SiteProperty.Address!.Contains(filterText!) || e.SiteProperty.StreetAndBuildingNumber!.Contains(filterText!) || e.SiteProperty.LandMark!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(propertyTitle), e => e.SiteProperty.PropertyTitle.Contains(propertyTitle))
                    .WhereIf(!string.IsNullOrWhiteSpace(hotelName), e => e.SiteProperty.HotelName.Contains(hotelName))
                    .WhereIf(bedroomsMin.HasValue, e => e.SiteProperty.Bedrooms >= bedroomsMin!.Value)
                    .WhereIf(bedroomsMax.HasValue, e => e.SiteProperty.Bedrooms <= bedroomsMax!.Value)
                    .WhereIf(bathroomsMin.HasValue, e => e.SiteProperty.Bathrooms >= bathroomsMin!.Value)
                    .WhereIf(bathroomsMax.HasValue, e => e.SiteProperty.Bathrooms <= bathroomsMax!.Value)
                    .WhereIf(numberOfBedMin.HasValue, e => e.SiteProperty.NumberOfBed >= numberOfBedMin!.Value)
                    .WhereIf(numberOfBedMax.HasValue, e => e.SiteProperty.NumberOfBed <= numberOfBedMax!.Value)
                    .WhereIf(floorMin.HasValue, e => e.SiteProperty.Floor >= floorMin!.Value)
                    .WhereIf(floorMax.HasValue, e => e.SiteProperty.Floor <= floorMax!.Value)
                    .WhereIf(maximumNumberOfGuestMin.HasValue, e => e.SiteProperty.MaximumNumberOfGuest >= maximumNumberOfGuestMin!.Value)
                    .WhereIf(maximumNumberOfGuestMax.HasValue, e => e.SiteProperty.MaximumNumberOfGuest <= maximumNumberOfGuestMax!.Value)
                    .WhereIf(livingroomsMin.HasValue, e => e.SiteProperty.Livingrooms >= livingroomsMin!.Value)
                    .WhereIf(livingroomsMax.HasValue, e => e.SiteProperty.Livingrooms <= livingroomsMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(propertyDescription), e => e.SiteProperty.PropertyDescription.Contains(propertyDescription))
                    .WhereIf(!string.IsNullOrWhiteSpace(hourseRules), e => e.SiteProperty.HourseRules.Contains(hourseRules))
                    .WhereIf(!string.IsNullOrWhiteSpace(importantInformation), e => e.SiteProperty.ImportantInformation.Contains(importantInformation))
                    .WhereIf(!string.IsNullOrWhiteSpace(address), e => e.SiteProperty.Address.Contains(address))
                    .WhereIf(!string.IsNullOrWhiteSpace(streetAndBuildingNumber), e => e.SiteProperty.StreetAndBuildingNumber.Contains(streetAndBuildingNumber))
                    .WhereIf(!string.IsNullOrWhiteSpace(landMark), e => e.SiteProperty.LandMark.Contains(landMark))
                    .WhereIf(pricePerNightMin.HasValue, e => e.SiteProperty.PricePerNight >= pricePerNightMin!.Value)
                    .WhereIf(pricePerNightMax.HasValue, e => e.SiteProperty.PricePerNight <= pricePerNightMax!.Value)
                    .WhereIf(isActive.HasValue, e => e.SiteProperty.IsActive == isActive)
                    .WhereIf(propertyTypeId != null && propertyTypeId != Guid.Empty, e => e.PropertyType != null && e.PropertyType.Id == propertyTypeId)
                    .WhereIf(governorateId != null && governorateId != Guid.Empty, e => e.Governorate != null && e.Governorate.Id == governorateId)
                    .WhereIf(propertyFeatureIds != null && propertyFeatureIds.Count > 0, e => e.SiteProperty.PropertyFeatures.Any(x => propertyFeatureIds.Contains(x.PropertyFeatureId)));

        }

    }
}