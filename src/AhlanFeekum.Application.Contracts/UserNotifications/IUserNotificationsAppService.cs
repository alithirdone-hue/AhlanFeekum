using AhlanFeekum.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using AhlanFeekum.Shared;

namespace AhlanFeekum.UserNotifications
{
    public partial interface IUserNotificationsAppService : IApplicationService
    {

        Task<PagedResultDto<UserNotificationWithNavigationPropertiesDto>> GetListAsync(GetUserNotificationsInput input);

        Task<UserNotificationWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<UserNotificationDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetUserProfileLookupAsync(LookupRequestDto input);

        Task<PagedResultDto<LookupDto<Guid>>> GetSitePropertyLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<UserNotificationDto> CreateAsync(UserNotificationCreateDto input);

        Task<UserNotificationDto> UpdateAsync(Guid id, UserNotificationUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(UserNotificationExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> usernotificationIds);

        Task DeleteAllAsync(GetUserNotificationsInput input);
        Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}