using System;
using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.UserProfiles
{
    public class CreatePaymentIntentDto
    {
        /// <summary>
        /// Amount in the smallest currency unit (e.g., cents for USD)
        /// </summary>
        [Required]
        public long Amount { get; set; }

        /// <summary>
        /// Three-letter ISO currency code (e.g., "usd", "eur")
        /// </summary>
        [Required]
        public string Currency { get; set; } = "usd";

        /// <summary>
        /// Optional description for the payment
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Optional metadata to attach to the payment intent
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string>? Metadata { get; set; }

        /// <summary>
        /// Customer email for receipt
        /// </summary>
        public string? ReceiptEmail { get; set; }

        /// <summary>
        /// Customer ID if exists
        /// </summary>
        public Guid? UserId { get; set; }
    }
}


