using Asp.Versioning;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.MobileResponses;
using AhlanFeekum.Authorizations;
using AhlanFeekum.PropertyMedias;
using System.Collections.Generic;

namespace AhlanFeekum.Controllers.SiteProperties
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Property")]
    [Route("api/mobile/properties")]

    public class PropertyController : AbpController
    {
        protected ISitePropertiesAppService _sitePropertiesAppService;
        protected IPropertyMediasAppService _propertyMediasAppService;
        public PropertyController(ISitePropertiesAppService sitePropertiesAppService, IPropertyMediasAppService propertyMediasAppService)
        {
            _sitePropertiesAppService = sitePropertiesAppService;
            _propertyMediasAppService = propertyMediasAppService;
        }

        [HttpPost("create-step-one")]
        public virtual Task<SitePropertyDto> CreateMobileAsync(SitePropertyCreateDto input)
        {
            return _sitePropertiesAppService.CreateAsync(input);
        }
        [HttpPost("create-step-two")]
        public virtual Task<SitePropertyDto> CreateStepTwoAsync(SitePropertyUpdateStepTwoDto input)
        {
            return _sitePropertiesAppService.UpdateAsync(input);
        }
        [HttpPost("upload-medias")]
        public virtual Task<MobileResponseDto> CreateStepTwoAsync(PropertyMediaCreateMobileDto input)
        {
            return _propertyMediasAppService.AddMediaToPropertyAsync(input);
        }

        [HttpGet]
        [Route("search-property")]
        public virtual Task<PagedResultDto<SitePropertyMobileDto>> GetListAsync(GetSitePropertiesMobileInput input)
        {
            return _sitePropertiesAppService.GetListMobileAsync(input);
        }

        [HttpPost]
        [Route("add-to-favorite/{id}")]
        public virtual Task<MobileResponseDto> AddToFavoriteAsync(Guid id)
        {
            return _sitePropertiesAppService.AddToFavoriteAsync(id);
        }

        [HttpPost]
        [Route("remove-from-favorite/{id}")]
        public virtual Task<MobileResponseDto> removeFromFavoriteAsync(Guid id)
        {
            return _sitePropertiesAppService.RemoveFromFavoriteAsync(id);
        }


    }
}