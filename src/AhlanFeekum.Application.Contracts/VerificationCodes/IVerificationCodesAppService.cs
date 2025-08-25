using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using AhlanFeekum.Shared;

namespace AhlanFeekum.VerificationCodes
{
    public partial interface IVerificationCodesAppService : IApplicationService
    {

        Task<PagedResultDto<VerificationCodeDto>> GetListAsync(GetVerificationCodesInput input);

        Task<VerificationCodeDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<VerificationCodeDto> CreateAsync(VerificationCodeCreateDto input);

        Task<VerificationCodeDto> UpdateAsync(Guid id, VerificationCodeUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(VerificationCodeExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> verificationcodeIds);

        Task DeleteAllAsync(GetVerificationCodesInput input);
        Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}