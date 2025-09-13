using AhlanFeekum.MobileResponses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AhlanFeekum.PropertyMedias
{
    public partial interface IPropertyMediasAppService
    {
        //Write your custom code here...
        Task<bool> UpdateSitePropertyMediasAsync(Guid sitePropertyId, List<PropertyMediaDto> input);

        Task<MobileResponseDto> AddMediaToPropertyAsync(List<PropertyMediaItemDto> input);
    }
}