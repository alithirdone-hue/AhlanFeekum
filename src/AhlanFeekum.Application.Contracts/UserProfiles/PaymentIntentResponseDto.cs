using System;
using System.Collections.Generic;

namespace AhlanFeekum.UserProfiles
{
    public class PaymentIntentResponseDto
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public long Amount { get; set; }
        public long AmountCapturable { get; set; }
        public long AmountReceived { get; set; }
        public string ClientSecret { get; set; }
        public string ConfirmationMethod { get; set; }
        public long Created { get; set; }
        public string Currency { get; set; }
        public string Customer { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public string ReceiptEmail { get; set; }
        public List<string> PaymentMethodTypes { get; set; }
    }
}


