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
using AhlanFeekum.Tickets;

namespace AhlanFeekum.Controllers.Payments
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Payment")]
    [Route("api/mobile/payments")]

    public class PaymentController : AbpController
    {
        protected IUserProfilesAppService _userProfilesAppService;
        protected ITicketsAppService _ticketsAppService;
        public PaymentController(IUserProfilesAppService userProfilesAppService, ITicketsAppService ticketsAppService)
        {
            _userProfilesAppService = userProfilesAppService;
            _ticketsAppService = ticketsAppService;
        }

 

    }
}