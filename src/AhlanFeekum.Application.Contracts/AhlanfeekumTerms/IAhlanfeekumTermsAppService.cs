using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using AhlanFeekum.Shared;

namespace AhlanFeekum.AhlanfeekumTerms
{
    public partial interface IAhlanfeekumTermsAppService : IApplicationService
    {
        Task<IRemoteStreamContent> GetFileAsync(GetFileInput input);

        Task<AppFileDescriptorDto> UploadFileAsync(IRemoteStreamContent input);

        Task<PagedResultDto<AhlanfeekumTermDto>> GetListAsync(GetAhlanfeekumTermsInput input);

        Task<AhlanfeekumTermDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<AhlanfeekumTermDto> CreateAsync(AhlanfeekumTermCreateDto input);

        Task<AhlanfeekumTermDto> UpdateAsync(Guid id, AhlanfeekumTermUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(AhlanfeekumTermExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> ahlanfeekumtermIds);

        Task DeleteAllAsync(GetAhlanfeekumTermsInput input);
        Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}