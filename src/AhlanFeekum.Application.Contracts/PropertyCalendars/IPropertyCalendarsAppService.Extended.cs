using AhlanFeekum.MobileResponses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AhlanFeekum.PropertyCalendars
{
    public partial interface IPropertyCalendarsAppService
    {
        //Write your custom code here...
        Task<MobileResponseDto> CreateManyAsync(List<PropertyCalendarItemDto> input);
    }
}