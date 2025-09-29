using Asp.Versioning;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using AhlanFeekum.UserProfiles;
using AhlanFeekum.MobileResponses;
using AhlanFeekum.Authorizations;
using AhlanFeekum.Reservations;
using System.Collections.Generic;

namespace AhlanFeekum.Controllers.Reservations
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Reservation")]
    [Route("api/mobile/reservations")]

    public class ReservationController : AbpController
    {
        protected IReservationsAppService _reservationsAppService;
        public ReservationController(IReservationsAppService reservationsAppService)
        {
            _reservationsAppService = reservationsAppService;
        }

        [AllowAnonymous]
        [HttpPost("create-reservation")]
        public virtual Task<ReservationMobileDto> RegisterAsync([FromForm] ReservationCreateMobileDto input)
        {
            return _reservationsAppService.CreateAsync(input);
        }

        [AllowAnonymous]
        [HttpGet("user-reservations/{id}")]
        public virtual Task<List<ReservationMobileDto>> UserReservationAsync(Guid id)
        {
            return _reservationsAppService.UserReservationsAsync(id);
        }
        [AllowAnonymous]
        [HttpGet("my-reservations")]
        public virtual Task<List<ReservationMobileDto>> MyReservationAsync()
        {
            return _reservationsAppService.UserReservationsAsync();
        }
       
    }
}