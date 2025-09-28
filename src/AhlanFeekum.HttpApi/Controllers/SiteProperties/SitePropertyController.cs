using AhlanFeekum.Authorizations;
using AhlanFeekum.MobileResponses;
using AhlanFeekum.PropertyCalendars;
using AhlanFeekum.PropertyEvaluations;
using AhlanFeekum.PropertyMedias;
using AhlanFeekum.SiteProperties;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

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
        protected IPropertyCalendarsAppService _propertyCalendarsAppService;
        protected IPropertyEvaluationsAppService _propertyEvaluationsAppService;
        public PropertyController(ISitePropertiesAppService sitePropertiesAppService, IPropertyMediasAppService propertyMediasAppService, IPropertyCalendarsAppService propertyCalendarsAppService, IPropertyEvaluationsAppService propertyEvaluationsAppService)
        {
            _sitePropertiesAppService = sitePropertiesAppService;
            _propertyMediasAppService = propertyMediasAppService;
            _propertyCalendarsAppService = propertyCalendarsAppService;
            _propertyEvaluationsAppService = propertyEvaluationsAppService;
        }

        [HttpPost("create-step-one")]
        public virtual Task<SitePropertyDto> CreateMobileAsync(SitePropertyCreateMobileDto input)
        {
            return _sitePropertiesAppService.CreateAsync(input);
        }
        [HttpPost("create-step-two")]
        public virtual Task<SitePropertyDto> CreateStepTwoAsync(SitePropertyUpdateStepTwoDto input)
        {
            return _sitePropertiesAppService.UpdateAsync(input);
        }
        [HttpPost("add-availability")]
        public virtual Task<MobileResponseDto> CreateManyPropertyCalendarAsync(List<PropertyCalendarItemDto> input)
        {
            return _propertyCalendarsAppService.CreateManyAsync(input);
        }

        [HttpPost("property-rating")]
        public virtual Task<PropertyEvaluationMobileDto> RatePropertyAsync(PropertyEvaluationCreateMobileDto input)
        {
            return _propertyEvaluationsAppService.CreateMobileAsync(input);
        }
        //[HttpPost("add-availability11")]
        //public virtual Task CreateManyProperty11CalendarAsync([FromBody] List<PropertyCalendarItemDto> input)
        //{
        //    return Task.CompletedTask;
        //}
        [HttpPost("upload-medias")]
        public virtual Task<MobileResponseDto> CreateStepTwoAsync([FromForm]List<PropertyMediaItemDto> input)
        {
            return _propertyMediasAppService.AddMediaToPropertyAsync(input);
        }

        [HttpPost("upload-one-media")]
        public virtual Task<MobileResponseDto> UploadOneMediaAsync([FromForm]PropertyMediaItemDto input)
        {
            List<PropertyMediaItemDto> mediaList = new List<PropertyMediaItemDto>()
            {
                input
            };
            return _propertyMediasAppService.AddMediaToPropertyAsync(mediaList);
        }
        [HttpPost("set-price")]
        public virtual Task<MobileResponseDto> SetPricePerNightAsync(SitePropertySetPriceDto input)
        {
            return _sitePropertiesAppService.SetPricePerNightAsync(input);
        }

        [HttpGet]
        [Route("search-property")]
        public virtual Task<PagedResultDto<SitePropertyListingMobileDto>> GetListAsync( GetSitePropertiesMobileInput input)
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
        public virtual Task<MobileResponseDto> RemoveFromFavoriteAsync(Guid id)
        {
            return _sitePropertiesAppService.RemoveFromFavoriteAsync(id);
        }

        [HttpGet("with-details/{id}")]
        public async Task<SitePropertyWithDetailsMobileDto> GetWithDetailsMobileAsync(Guid id)
        {
            return await _sitePropertiesAppService.GetSitePropertyWithDetailsAsync(id);
        }

        [HttpGet("property-rating/{id}")]
        public async Task<SitePropertyWithDetailsMobileDto> GetPropertyRatingAsync(Guid id)
        {
            return await _sitePropertiesAppService.GetSitePropertyWithDetailsAsync(id);
        }


    }
}