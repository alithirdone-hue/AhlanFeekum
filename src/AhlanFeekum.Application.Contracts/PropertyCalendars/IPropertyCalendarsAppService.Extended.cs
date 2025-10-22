using AhlanFeekum.MobileResponses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace AhlanFeekum.PropertyCalendars
{
    public partial interface IPropertyCalendarsAppService
    {
        //Write your custom code here...
        Task<MobileResponseDto> CreateManyAsync(List<PropertyCalendarItemDto> input);

        Task<MobileResponseDto> UpdateManyAsync(List<PropertyCalendarItemDto> input);

        Task<PagedResultDto<PropertyCalendarMobileDto>> GetListMobileAsync(GetPropertyCalendarsInput input);
        Task<PagedResultDto<PropertyCalendarStatus>> GetListCalendarStatusAsync(GetPropertyCalendarsInput input);
    }
}