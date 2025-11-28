using AhlanFeekum.UserPayments;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.UserPayments
{
    public abstract class UserPaymentCreateDtoBase
    {
        public long Amount { get; set; }
        [Required]
        public string Currency { get; set; } = null!;
        public string? Description { get; set; }
        public string? ReceiptEmail { get; set; }
        public long AmountCapturable { get; set; } = 0;
        public long AmountReceived { get; set; } = 0;
        public string? ConfirmationMethod { get; set; }
        public UserPaymentStatus Status { get; set; } = ((UserPaymentStatus[])Enum.GetValues(typeof(UserPaymentStatus)))[0];
        [Required]
        public string StripPaymentId { get; set; } = null!;
        [Required]
        public string StripClientSecret { get; set; } = null!;
        [Required]
        public string Created { get; set; } = null!;
        public Guid UserProfileId { get; set; }
        public Guid ReservationId { get; set; }
    }
}