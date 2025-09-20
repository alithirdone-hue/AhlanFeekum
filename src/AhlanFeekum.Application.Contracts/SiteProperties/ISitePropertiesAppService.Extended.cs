using AhlanFeekum.MobileResponses;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace AhlanFeekum.SiteProperties
{
    public partial interface ISitePropertiesAppService
    {
        //Write your custom code here...
        Task<SitePropertyWithDetailsMobileDto> GetSitePropertyWithDetailsAsync(Guid id);
        Task<SitePropertyDto> UpdateAsync(SitePropertyUpdateStepTwoDto input);
        Task<MobileResponseDto> SetPricePerNightAsync(SitePropertySetPriceDto input);
        Task<PagedResultDto<SitePropertyListingMobileDto>> GetListMobileAsync(GetSitePropertiesMobileInput input);
        Task<MobileResponseDto> AddToFavoriteAsync(Guid id);
        Task<MobileResponseDto> RemoveFromFavoriteAsync(Guid id);
    }
}