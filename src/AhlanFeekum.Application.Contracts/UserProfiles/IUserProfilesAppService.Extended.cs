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
        Task<MobileResponseDto> VerifyAsync(VerifyRequestDto input);

        Task<MobileResponseDto> SendSecretKeyPhoneAsync(string input);
        Task<MobileResponseDto> VerifyPhoneAsync(PhoneVerifyRequestDto input);

        Task<MobileResponseDto> RegisterAsync(RegisterCreateMobileDto input);
        Task<MobileResponseDto> UpdateMyProfileAsync(UserProfileUpdateMobileDto input);
        Task<MobileResponseDto> RequestPasswordResetAsync(PasswordResetRequestDto input);
        Task<MobileResponseDto> ConfirmPasswordResetAsync(PasswordConfirmResetRequestDto input);
        Task<MobileResponseDto> ChangePasswordAsync(PasswordChangeRequestDto input);

        Task<HomePageDto> GetHomePageAsync();
        Task<UserProfileWithDetailsMobileDto> GetWithDetailsAsync(Guid? id);

        // Stripe Payment Methods
        Task<PaymentIntentResponseDto> CreatePaymentIntentAsync(CreatePaymentIntentDto input);
        Task<PaymentIntentResponseDto> GetPaymentIntentAsync(string paymentIntentId);
        Task<PaymentIntentResponseDto> ConfirmPaymentIntentAsync(ConfirmPaymentDto input);
        Task<StripeWebhookEventDto> HandleStripeWebhookAsync(string json, string stripeSignature);
        Task<PaymentSummaryResponseDto> GetPaymentSummaryAsync(PaymentSummaryRequestDto input);

        // Stripe Payment with Hold (Manual Capture)
        Task<PaymentIntentResponseDto> CreatePaymentIntentWithHoldAsync(CreatePaymentIntentDto input);
        Task<PaymentIntentResponseDto> CapturePaymentAsync(string paymentIntentId);
        Task<PaymentIntentResponseDto> CancelPaymentAsync(string paymentIntentId);
        Task ProcessPendingPaymentsAsync();

        Task<MobileResponseDto> CheckUserExistEmailOrPhone(string input);

        /// <summary>
        /// Deletes/deactivates the current user account. Performs a soft delete on UserProfile and deactivates IdentityUser to prevent login.
        /// </summary>
        Task<MobileResponseDto> DeleteCurrentUserAsync();

    }
}