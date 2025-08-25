using AhlanFeekum.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using AhlanFeekum.Shared;

namespace AhlanFeekum.PropertyMedias
{
    public partial interface IPropertyMediasAppService : IApplicationService
    {

        Task<PagedResultDto<PropertyMediaWithNavigationPropertiesDto>> GetListAsync(GetPropertyMediasInput input);

        Task<PropertyMediaWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<PropertyMediaDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetSitePropertyLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<PropertyMediaDto> CreateAsync(PropertyMediaCreateDto input);

        Task<PropertyMediaDto> UpdateAsync(Guid id, PropertyMediaUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(PropertyMediaExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> propertymediaIds);

        Task DeleteAllAsync(GetPropertyMediasInput input);
        Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}