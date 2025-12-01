using AhlanFeekum.UserPayments;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.UserPayments
{
    public abstract class UserPaymentCreateDtoBase
    {
        public long Amount { get; set; }
        public string? Currency { get; set; }
        public string? Description { get; set; }
        public string? ReceiptEmail { get; set; }
        public long AmountCapturable { get; set; } = 0;
        public long AmountReceived { get; set; } = 0;
        public string? ConfirmationMethod { get; set; }
        public UserPaymentStatus Status { get; set; } = ((UserPaymentStatus[])Enum.GetValues(typeof(UserPaymentStatus)))[0];
        public string? StripPaymentId { get; set; }
        public string? StripClientSecret { get; set; }
        public DateTime Created { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod)))[0];
        public Guid UserProfileId { get; set; }
        public Guid ReservationId { get; set; }
    }
}