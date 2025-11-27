using AhlanFeekum.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using AhlanFeekum.Shared;

namespace AhlanFeekum.UserPayments
{
    public partial interface IUserPaymentsAppService : IApplicationService
    {

        Task<PagedResultDto<UserPaymentWithNavigationPropertiesDto>> GetListAsync(GetUserPaymentsInput input);

        Task<UserPaymentWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<UserPaymentDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetUserProfileLookupAsync(LookupRequestDto input);

        Task<PagedResultDto<LookupDto<Guid>>> GetReservationLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<UserPaymentDto> CreateAsync(UserPaymentCreateDto input);

        Task<UserPaymentDto> UpdateAsync(Guid id, UserPaymentUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(UserPaymentExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> userpaymentIds);

        Task DeleteAllAsync(GetUserPaymentsInput input);
        Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}