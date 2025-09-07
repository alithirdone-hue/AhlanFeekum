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
            return (await GetDbSetAsync())
                .Select(homePage => new HomePage
                {
                    SpecialAdvertisments = dbContext.SpecialAdvertisments.Where(s=>s.IsActive)
                                            .Select(specialAdvertisment => new SpecialAdvertismentWithNavigationProperties
                                            {
                                                SpecialAdvertisment = specialAdvertisment,
                                                SiteProperty = dbContext.SiteProperties.Where(s=>s.Id == specialAdvertisment.SitePropertyId).FirstOrDefault()
                                            }).ToList(),
                    SitePropertyWithDetails = 
                    (from siteProperty in (dbContext.SiteProperties.Include(p=>p.PropertyFeatures))
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
                       Medias = dbContext.PropertyMedias.Where(pm=>pm.SitePropertyId == siteProperty.Id).OrderBy(pm=>pm.Order).ToList(),
                       IsFavorite = userId == null ? false : dbContext.FavoriteProperties.Any(p => p.SitePropertyId == siteProperty.Id && p.UserProfileId == userId),
                   }).ToList(),
                    Governorates = dbContext.Governorates.OrderBy(g => g.Order).ToList(),
                    OnlyForYouSection = dbContext.OnlyForYouSections.FirstOrDefault()
                }).FirstOrDefault();
        }
    }
}