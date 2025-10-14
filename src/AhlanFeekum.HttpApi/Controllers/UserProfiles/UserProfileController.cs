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

namespace AhlanFeekum.Controllers.UserProfiles
{
    [RemoteService]
    [Area("app")]
    [ControllerName("UserProfile")]
    [Route("api/mobile/user-profiles")]

    public class UserProfileController : AbpController
    {
        protected IUserProfilesAppService _userProfilesAppService;
        protected ITicketsAppService _ticketsAppService;
        public UserProfileController(IUserProfilesAppService userProfilesAppService, ITicketsAppService ticketsAppService)
        {
            _userProfilesAppService = userProfilesAppService;
            _ticketsAppService = ticketsAppService;
        }

        [AllowAnonymous]
        [HttpPost("register-user")]
        public virtual Task<MobileResponseDto> RegisterAsync([FromForm] RegisterCreateMobileDto input)
        {
            return _userProfilesAppService.RegisterAsync(input);
        }

        [AllowAnonymous]
        [HttpPost("password-reset-request")]
        public virtual Task<MobileResponseDto> RequestPasswordResetAsync(PasswordResetRequestDto input)
        {
            return _userProfilesAppService.RequestPasswordResetAsync(input);
        }
        [AllowAnonymous]
        [HttpPost("confirm-password-reset")]
        public virtual Task<MobileResponseDto> ConfirmPasswordResetAsync(PasswordConfirmResetRequestDto input)
        {
            return _userProfilesAppService.ConfirmPasswordResetAsync(input);
        }

        [AllowAnonymous]
        [HttpPost("password-change")]
        public virtual Task<MobileResponseDto> ChangePasswordAsync(PasswordChangeRequestDto input)
        {
            return _userProfilesAppService.ChangePasswordAsync(input);
        }

        [HttpPost("update-my-profile")]
        public virtual Task<MobileResponseDto> UpdateMyProfileAsync([FromForm] UserProfileUpdateMobileDto input)
        {
            return _userProfilesAppService.UpdateMyProfileAsync(input);
        }
        [HttpPost("send-secret-key-email")]
        public virtual Task<MobileResponseDto> SendSecretKeyEmail(string email)
        {
            return _userProfilesAppService.SendSecretKeyEmailAsync(email);
        }
        [AllowAnonymous]
        [HttpPost("verify")]
        public virtual Task<MobileResponseDto> VerifyAsync(VerifyRequestDto input)
        {
            return _userProfilesAppService.VerifyAsync(input);
        }
        [HttpPost("send-secret-key-phone")]
        public virtual Task<MobileResponseDto> SendSecretKeyPhone(string email)
        {
            return _userProfilesAppService.SendSecretKeyPhoneAsync(email);
        }

        [AllowAnonymous]
        [HttpPost("verify-phone")]
        public virtual Task<MobileResponseDto> VerifyPhoneAsync(PhoneVerifyRequestDto input)
        {
            return _userProfilesAppService.VerifyPhoneAsync(input);
        }

        [HttpGet("home")]
        public virtual Task<HomePageDto> GetHomePageAsync()
        {
            return _userProfilesAppService.GetHomePageAsync();
        }


        [HttpGet]
        [Route("user-profile-details/{id:guid?}")]
        public virtual Task<UserProfileWithDetailsMobileDto> GetWithDetailsAsync(Guid? id)
        {
            return _userProfilesAppService.GetWithDetailsAsync(id);
        }

        [AllowAnonymous]
        [HttpPost("create-ticket")]
        public virtual Task<TicketDto> CreateTicketAsync(TicketCreateMobileDto input)
        {
            return _ticketsAppService.CreateAsync(input);
        }

    }
}