using AhlanFeekum.MobileResponses;
using AhlanFeekum.Shared;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.UserProfiles;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace AhlanFeekum.Controllers.Lookups
{
    [RemoteService]
    [Area("app")]
    [ControllerName("LookUp")]
    [Route("api/mobile/lookups")]

    public class LookUpController : AbpController
    {
        protected ISitePropertiesAppService _sitePropertiesAppService;
        public LookUpController(ISitePropertiesAppService sitePropertiesAppService)
        {
            _sitePropertiesAppService = sitePropertiesAppService;
        }

        [HttpGet]
        [Route("property-types")]
        public virtual Task<PagedResultDto<LookupDto<Guid>>> GetPropertyTypeLookupAsync(LookupRequestDto lookupRequestDto)
        {
            return _sitePropertiesAppService.GetPropertyTypeLookupAsync(lookupRequestDto);
        }
        [HttpGet]
        [Route("property-features")]
        public virtual Task<PagedResultDto<LookupDto<Guid>>> GetPropertyFeatureLookupAsync(LookupRequestDto lookupRequestDto)
        {
            return _sitePropertiesAppService.GetPropertyFeatureLookupAsync(lookupRequestDto);
        }

        [HttpGet]
        [Route("governates")]
        public virtual Task<PagedResultDto<LookupDto<Guid>>> GetGovernateLookupAsync(LookupRequestDto lookupRequestDto)
        {
            return _sitePropertiesAppService.GetGovernorateLookupAsync(lookupRequestDto);
        }
        [HttpGet]
        [Route("statuses")]
        public virtual Task<PagedResultDto<LookupDto<Guid>>> GetStatusLookupAsync(LookupRequestDto lookupRequestDto)
        {
            return _sitePropertiesAppService.GetStatusLookupAsync(lookupRequestDto);
        }

    }
}