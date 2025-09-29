using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AhlanFeekum.Reservations
{
    public partial interface IReservationsAppService
    {
        //Write your custom code here...
        Task<ReservationMobileDto> CreateAsync(ReservationCreateMobileDto input);
        Task<List<ReservationMobileDto>> UserReservationsAsync(Guid? userId = null);
    }
}