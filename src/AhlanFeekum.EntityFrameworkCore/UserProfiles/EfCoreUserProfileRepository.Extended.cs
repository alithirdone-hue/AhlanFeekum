using AhlanFeekum.EntityFrameworkCore;
using AhlanFeekum.Governorates;
using AhlanFeekum.PropertyFeatures;
using AhlanFeekum.PropertyTypes;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.SpecialAdvertisments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace AhlanFeekum.UserProfiles
{
    public class EfCoreUserProfileRepository : EfCoreUserProfileRepositoryBase, IUserProfileRepository
    {
        public EfCoreUserProfileRepository(IDbContextProvider<AhlanFeekumDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<HomePage> GetHomePageAsync(Guid? userId)
        {
            var dbContext = await GetDbContextAsync();
            Guid? hotelPropertyTypeId = null;
            PropertyType propertyType = dbContext.PropertyTypes.FirstOrDefault(p => p.Title.ToLower() == "hotel");
            if (propertyType != null)
                hotelPropertyTypeId = propertyType.Id;
            return (await GetDbSetAsync())
                .Select(homePage => new HomePage
                {

                    UserProfile = userId != null ? dbContext.UserProfiles.FirstOrDefault(u => u.Id == userId) : null,
                    SpecialAdvertisments = dbContext.SpecialAdvertisments.Where(s=>s.IsActive).OrderBy(s=>s.Order)
                                            .Select(specialAdvertisment => new SpecialAdvertismentWithNavigationProperties
                                            {
                                                SpecialAdvertisment = specialAdvertisment,
                                                SiteProperty = dbContext.SiteProperties.Where(s=>s.Id == specialAdvertisment.SitePropertyId).FirstOrDefault()
                                            }).ToList(),
                    SiteProperties =
                    (from siteProperty in (dbContext.SiteProperties.Include(p => p.PropertyFeatures))
                     select new SitePropertyWithDetails
                     {
                         SiteProperty = siteProperty,
                         PropertyType = null,
                         Governorate = null,
                         PropertyFeatures = null,
                         MainImage = dbContext.PropertyMedias.Where(pm => pm.SitePropertyId == siteProperty.Id).OrderBy(pm => pm.Order).FirstOrDefault(),
                         Medias = null,
                         IsFavorite = userId == null ? false : dbContext.FavoriteProperties.Any(p => p.SitePropertyId == siteProperty.Id && p.UserProfileId == userId),
                     }).ToList(),
                    HighlyRated =
                    (from siteProperty in (dbContext.SiteProperties.Include(p => p.PropertyFeatures))
                     select new SitePropertyWithDetails
                     {
                         SiteProperty = siteProperty,
                         PropertyType = null,
                         Governorate = null,
                         PropertyFeatures = null,
                         MainImage = dbContext.PropertyMedias.Where(pm => pm.SitePropertyId == siteProperty.Id).OrderBy(pm => pm.Order).FirstOrDefault(),
                         Medias = null,
                         IsFavorite = userId == null ? false : dbContext.FavoriteProperties.Any(p => p.SitePropertyId == siteProperty.Id && p.UserProfileId == userId),
                         AverageRating = dbContext.PropertyEvaluations.Where(p=>p.SitePropertyId == siteProperty.Id).Average(e => (e.Cleanliness + e.PriceAndValue + e.Location + e.Accuracy + e.Attitude) / 5.0)
                     }).ToList(),
                    // SitePropertyWithDetails = 
                    // (from siteProperty in (dbContext.SiteProperties.Include(p=>p.PropertyFeatures))
                    //join propertyType in dbContext.PropertyTypes on siteProperty.PropertyTypeId equals propertyType.Id into propertyTypes
                    //from propertyType in propertyTypes.DefaultIfEmpty()
                    //join governorate in dbContext.Governorates on siteProperty.GovernorateId equals governorate.Id into governorates
                    //from governorate in governorates.DefaultIfEmpty()
                    //select new SitePropertyWithDetails
                    //{
                    //    SiteProperty = siteProperty,
                    //    PropertyType = propertyType,
                    //    Governorate = governorate,
                    //    PropertyFeatures = (from sitePropertyPropertyFeature in siteProperty.PropertyFeatures
                    //                 join _propertyFeature in dbContext.Set<PropertyFeature>() on sitePropertyPropertyFeature.PropertyFeatureId equals _propertyFeature.Id
                    //                 select _propertyFeature).ToList(),
                    //    Medias = dbContext.PropertyMedias.Where(pm=>pm.SitePropertyId == siteProperty.Id).OrderBy(pm=>pm.Order).ToList(),
                    //    IsFavorite = userId == null ? false : dbContext.FavoriteProperties.Any(p => p.SitePropertyId == siteProperty.Id && p.UserProfileId == userId),
                    //}).ToList(),
                    Governorates = dbContext.Governorates.OrderBy(g => g.Order).ToList(),
                    HotelsOfTheWeek =
                    (from siteProperty in (dbContext.SiteProperties.Where(p=>p.PropertyTypeId == hotelPropertyTypeId).Include(p => p.PropertyFeatures))
                     from user in dbContext.UserProfiles where siteProperty.OwnerId == user.Id
                     select new UserProfile(user.Id,user.IdentityRoleId, user.IdentityUserId,user.Name, user.IsSuperHost,user.Email,
                     user.PhoneNumber, user.Latitude, user.Longitude,user.Address, user.ProfilePhoto)
                     {
                         AverageRating = dbContext.PropertyEvaluations.Where(p => p.SitePropertyId == siteProperty.Id).Average(e => (e.Cleanliness + e.PriceAndValue + e.Location + e.Accuracy + e.Attitude) / 5.0)
                     }).Take(2).ToList(),
                    OnlyForYouSection = dbContext.OnlyForYouSections.FirstOrDefault()
                }).FirstOrDefault();
        }

        public async Task<UserProfileWithDetails> GetWithDetailsAsync( Guid userId)
        {
            var dbContext = await GetDbContextAsync();

            return (from userProfile in (await GetDbSetAsync()).Where(b => b.Id == userId)
                   select new UserProfileWithDetails
                   {
                       UserProfile = userProfile,
                       MyProperties = (
                                             from siteProperty in (dbContext.SiteProperties).Where(b => b.OwnerId == userId).Include(x => x.PropertyFeatures)
                                             join propertyType in dbContext.PropertyTypes on siteProperty.PropertyTypeId equals propertyType.Id into propertyTypes
                                             from propertyType in propertyTypes.DefaultIfEmpty()
                                             join governorate in dbContext.Governorates on siteProperty.GovernorateId equals governorate.Id into governorates
                                             from governorate in governorates.DefaultIfEmpty()
                                             select new SitePropertyWithDetails
                                             {
                                                 SiteProperty = siteProperty,
                                                 PropertyType = propertyType,
                                                 Governorate = governorate,
                                                 PropertyFeatures = (from sitePropertyPropertyFeature in siteProperty.PropertyFeatures
                                                                     join _propertyFeature in dbContext.Set<PropertyFeature>() on sitePropertyPropertyFeature.PropertyFeatureId equals _propertyFeature.Id
                                                                     select _propertyFeature).ToList(),
                                                 Medias = dbContext.PropertyMedias.Where(pm => pm.SitePropertyId == siteProperty.Id).OrderBy(pm => pm.Order).ToList(),
                                                 IsFavorite = userId == null ? false : dbContext.FavoriteProperties.Any(p => p.SitePropertyId == siteProperty.Id && p.UserProfileId == userId),
                                             }).ToList(),

                       FavoriteProperties = (
                                            from fav in (dbContext.FavoriteProperties).Where(f =>f.UserProfileId == userId)
                                             join siteProperty in (dbContext.SiteProperties) on fav.SitePropertyId equals siteProperty.Id into siteProperties
                                             from siteProperty in siteProperties.DefaultIfEmpty()
                                             join propertyType in dbContext.PropertyTypes on siteProperty.PropertyTypeId equals propertyType.Id into propertyTypes
                                             from propertyType in propertyTypes.DefaultIfEmpty()
                                             join governorate in dbContext.Governorates on siteProperty.GovernorateId equals governorate.Id into governorates
                                             from governorate in governorates.DefaultIfEmpty()
                                             select new SitePropertyWithDetails
                                             {
                                                 SiteProperty = siteProperty,
                                                 PropertyType = propertyType,
                                                 Governorate = governorate,
                                                 PropertyFeatures = (from sitePropertyPropertyFeature in siteProperty.PropertyFeatures
                                                                     join _propertyFeature in dbContext.Set<PropertyFeature>() on sitePropertyPropertyFeature.PropertyFeatureId equals _propertyFeature.Id
                                                                     select _propertyFeature).ToList(),
                                                 Medias = dbContext.PropertyMedias.Where(pm => pm.SitePropertyId == siteProperty.Id).OrderBy(pm => pm.Order).ToList(),
                                                 IsFavorite = userId == null ? false : dbContext.FavoriteProperties.Any(p => p.SitePropertyId == siteProperty.Id && p.UserProfileId == userId),
                                             }).ToList(),


                   }).FirstOrDefault();
        }
    }
}