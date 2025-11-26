using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.UserProfiles
{
    public class ConfirmPaymentDto
    {
        /// <summary>
        /// The PaymentIntent ID to confirm
        /// </summary>
        [Required]
        public string PaymentIntentId { get; set; }

        /// <summary>
        /// The payment method ID (optional if already attached)
        /// </summary>
        public string? PaymentMethodId { get; set; }
    }
}


