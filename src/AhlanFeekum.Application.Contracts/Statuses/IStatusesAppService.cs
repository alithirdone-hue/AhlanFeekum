using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using AhlanFeekum.Shared;

namespace AhlanFeekum.Statuses
{
    public partial interface IStatusesAppService : IApplicationService
    {

        Task<PagedResultDto<StatusDto>> GetListAsync(GetStatusesInput input);

        Task<StatusDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<StatusDto> CreateAsync(StatusCreateDto input);

        Task<StatusDto> UpdateAsync(Guid id, StatusUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(StatusExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> statusIds);

        Task DeleteAllAsync(GetStatusesInput input);
        Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}