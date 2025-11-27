using AhlanFeekum.UserPayments;
using System;

namespace AhlanFeekum.UserPayments
{
    public abstract class UserPaymentExcelDtoBase
    {
        public long Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string? Description { get; set; }
        public string? ReceiptEmail { get; set; }
        public long AmountCapturable { get; set; }
        public long AmountReceived { get; set; }
        public string? ConfirmationMethod { get; set; }
        public UserPaymentStatus Status { get; set; }
        public string StripPaymentId { get; set; } = null!;
        public string StripClientSecret { get; set; } = null!;
    }
}