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
using AhlanFeekum.AhlanfeekumTerms;

namespace AhlanFeekum.Controllers.AhlanfeekumSettings
{
    [RemoteService]
    [Area("app")]
    [ControllerName("AhlanfeekumSetting")]
    [Route("api/mobile/settings")]

    public class AhlanfeekumSettingController : AbpController
    {
        protected IAhlanfeekumTermsAppService _ahlanfeekumTermsAppService;
        public AhlanfeekumSettingController(IAhlanfeekumTermsAppService ahlanfeekumTermsAppService)
        {
            _ahlanfeekumTermsAppService = ahlanfeekumTermsAppService;
        }

        [AllowAnonymous]
        [HttpPost("get")]
        public virtual Task<AhlanfeekumTermMobileDto> GetTerms()
        {
           return _ahlanfeekumTermsAppService.GetAsync();
        }

      
       
    }
}