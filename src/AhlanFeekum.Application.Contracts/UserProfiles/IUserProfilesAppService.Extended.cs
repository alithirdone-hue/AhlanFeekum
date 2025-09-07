using AhlanFeekum.MobileResponses;
using AhlanFeekum.OnlyForYouSections;
using System;
using System.Threading.Tasks;

namespace AhlanFeekum.UserProfiles
{
    public partial interface IUserProfilesAppService
    {
        //Write your custom code here...
        Task<MobileResponseDto> SendSecretKeyEmailAsync(string input);
        Task<MobileResponseDto> SendSecretKeyPhoneAsync(string input);
        Task<MobileResponseDto> VerifyAsync(VerifyRequestDto input);

        Task<MobileResponseDto> RegisterAsync(RegisterCreateMobileDto input);
        Task<MobileResponseDto> RequestPasswordResetAsync(PasswordResetRequestDto input);
        Task<MobileResponseDto> ConfirmPasswordResetAsync(PasswordConfirmResetRequestDto input);
        Task<MobileResponseDto> ChangePasswordAsync(PasswordChangeRequestDto input);

        Task<HomePageDto> GetHomePageAsync();

    }
}