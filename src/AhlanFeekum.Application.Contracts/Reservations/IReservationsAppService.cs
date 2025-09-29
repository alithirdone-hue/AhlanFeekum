using AhlanFeekum.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using AhlanFeekum.Shared;

namespace AhlanFeekum.Reservations
{
    public partial interface IReservationsAppService : IApplicationService
    {

        Task<PagedResultDto<ReservationWithNavigationPropertiesDto>> GetListAsync(GetReservationsInput input);

        Task<ReservationWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<ReservationDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetUserProfileLookupAsync(LookupRequestDto input);

        Task<PagedResultDto<LookupDto<Guid>>> GetSitePropertyLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<ReservationDto> CreateAsync(ReservationCreateDto input);

        Task<ReservationDto> UpdateAsync(Guid id, ReservationUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ReservationExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> reservationIds);

        Task DeleteAllAsync(GetReservationsInput input);
        Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}