using System.Collections.Generic;

namespace AhlanFeekum.UserProfiles
{
    public class StripeWebhookEventDto
    {
        public string EventType { get; set; }
        public string PaymentIntentId { get; set; }
        public string Status { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public string CustomerId { get; set; }
        public string ReceiptEmail { get; set; }
    }
}

