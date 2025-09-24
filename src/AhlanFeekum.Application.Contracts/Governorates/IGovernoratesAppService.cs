using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using AhlanFeekum.Shared;

namespace AhlanFeekum.Governorates
{
    public partial interface IGovernoratesAppService : IApplicationService
    {
        Task<IRemoteStreamContent> GetFileAsync(GetFileInput input);

        Task<AppFileDescriptorDto> UploadFileAsync(IRemoteStreamContent input);

        Task<PagedResultDto<GovernorateDto>> GetListAsync(GetGovernoratesInput input);

        Task<GovernorateDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<GovernorateDto> CreateAsync(GovernorateCreateDto input);

        Task<GovernorateDto> UpdateAsync(Guid id, GovernorateUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(GovernorateExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> governorateIds);

        Task DeleteAllAsync(GetGovernoratesInput input);
        Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}