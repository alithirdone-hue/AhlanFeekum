using AhlanFeekum.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using AhlanFeekum.Shared;

namespace AhlanFeekum.CashPayments
{
    public partial interface ICashPaymentsAppService : IApplicationService
    {

        Task<PagedResultDto<CashPaymentWithNavigationPropertiesDto>> GetListAsync(GetCashPaymentsInput input);

        Task<CashPaymentWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<CashPaymentDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetUserProfileLookupAsync(LookupRequestDto input);

        Task<PagedResultDto<LookupDto<Guid>>> GetReservationLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<CashPaymentDto> CreateAsync(CashPaymentCreateDto input);

        Task<CashPaymentDto> UpdateAsync(Guid id, CashPaymentUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(CashPaymentExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> cashpaymentIds);

        Task DeleteAllAsync(GetCashPaymentsInput input);
        Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}