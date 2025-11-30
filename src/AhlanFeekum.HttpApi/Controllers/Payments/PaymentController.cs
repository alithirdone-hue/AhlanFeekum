using Asp.Versioning;
using System;
using System.IO;
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

        /// <summary>
        /// Creates a new Stripe payment intent
        /// </summary>
        /// <param name="input">Payment intent creation details</param>
        /// <returns>Payment intent with client secret for frontend processing</returns>
        //[HttpPost("create-intent")]
        //[AllowAnonymous]
        //public virtual async Task<PaymentIntentResponseDto> CreatePaymentIntentAsync([FromBody] CreatePaymentIntentDto input)
        //{
        //    return await _userProfilesAppService.CreatePaymentIntentAsync(input);
        //}

        /// <summary>
        /// Retrieves an existing payment intent by ID
        /// </summary>
        /// <param name="paymentIntentId">The Stripe payment intent ID</param>
        /// <returns>Payment intent details</returns>
        [HttpGet("{paymentIntentId}")]
        [AllowAnonymous]
        public virtual async Task<PaymentIntentResponseDto> GetPaymentIntentAsync(string paymentIntentId)
        {
            return await _userProfilesAppService.GetPaymentIntentAsync(paymentIntentId);
        }



        /// <summary>
        /// Stripe webhook endpoint to receive payment events
        /// </summary>
        /// <returns>Webhook event details</returns>
        [HttpPost("webhook")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> StripeWebhookAsync()
        {
            try
            {
                // Read the raw body
                using var reader = new StreamReader(HttpContext.Request.Body);
                var json = await reader.ReadToEndAsync();

                // Get the Stripe signature from header
                var stripeSignature = HttpContext.Request.Headers["Stripe-Signature"].ToString();

                if (string.IsNullOrEmpty(stripeSignature))
                {
                    return BadRequest(new { error = "Missing Stripe-Signature header" });
                }

                // Process the webhook
                var result = await _userProfilesAppService.HandleStripeWebhookAsync(json, stripeSignature);

                return Ok(new { received = true, eventType = result.EventType });
            }
            catch (Exception ex)
            {
                // Return 400 for signature verification failures
                // Stripe will retry webhooks that return non-2xx status codes
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Gets payment summary for the current user grouped by month
        /// </summary>
        /// <param name="input">Date range for the payment summary</param>
        /// <returns>Monthly payments dictionary and total payment amount</returns>
        [HttpPost("summary")]
        public virtual async Task<PaymentSummaryResponseDto> GetPaymentSummaryAsync([FromBody] PaymentSummaryRequestDto input)
        {
            return await _userProfilesAppService.GetPaymentSummaryAsync(input);
        }

        #region Payment with Hold (Manual Capture - 24 Hour Auto-Process)

        /// <summary>
        /// Creates a payment intent with manual capture (holds payment for 24 hours)
        /// Payment will be automatically captured or canceled after 24 hours based on reservation status
        /// </summary>
        /// <param name="input">Payment intent creation details</param>
        /// <returns>Payment intent with client secret for frontend processing</returns>
        [HttpPost("create-intent-with-hold")]
        [AllowAnonymous]
        public virtual async Task<PaymentIntentResponseDto> CreatePaymentIntentWithHoldAsync([FromBody] CreatePaymentIntentDto input)
        {
            return await _userProfilesAppService.CreatePaymentIntentWithHoldAsync(input);
        }
        /// <summary>
        /// Confirms a payment intent with a payment method
        /// </summary>
        /// <param name="input">Confirmation details including payment method</param>
        /// <returns>Updated payment intent status</returns>
        [HttpPost("confirm")]
        [AllowAnonymous]
        public virtual async Task<PaymentIntentResponseDto> ConfirmPaymentIntentAsync([FromBody] ConfirmPaymentDto input)
        {
            return await _userProfilesAppService.ConfirmPaymentIntentAsync(input);
        }
        /// <summary>
        /// Captures a held payment (completes the transaction)
        /// </summary>
        /// <param name="paymentIntentId">The Stripe payment intent ID to capture</param>
        /// <returns>Updated payment intent with succeeded status</returns>
        [HttpPost("capture/{paymentIntentId}")]
        [Authorize]
        public virtual async Task<PaymentIntentResponseDto> CapturePaymentAsync(string paymentIntentId)
        {
            return await _userProfilesAppService.CapturePaymentAsync(paymentIntentId);
        }

        /// <summary>
        /// Cancels a held payment (releases the authorization)
        /// </summary>
        /// <param name="paymentIntentId">The Stripe payment intent ID to cancel</param>
        /// <returns>Updated payment intent with canceled status</returns>
        [HttpPost("cancel/{paymentIntentId}")]
        [Authorize]
        public virtual async Task<PaymentIntentResponseDto> CancelPaymentAsync(string paymentIntentId)
        {
            return await _userProfilesAppService.CancelPaymentAsync(paymentIntentId);
        }

        /// <summary>
        /// Background job endpoint to process pending payments after 24 hours
        /// Should be called by a scheduled job (e.g., Hangfire, cron job)
        /// </summary>
        /// <returns>Success status</returns>
        //[HttpPost("process-pending")]
        //[Authorize]
        //public virtual async Task<IActionResult> ProcessPendingPaymentsAsync()
        //{
        //    try
        //    {
        //        await _userProfilesAppService.ProcessPendingPaymentsAsync();
        //        return Ok(new { success = true, message = "Pending payments processed successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { success = false, error = ex.Message });
        //    }
        //}

        #endregion
    }
}