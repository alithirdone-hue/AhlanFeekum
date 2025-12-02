using AhlanFeekum.CashPayments;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.CashPayments
{
    public abstract class CashPaymentCreateDtoBase
    {
        public long Amount { get; set; }
        [Required]
        public string Currency { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public string? Description { get; set; }
        public CashPaymentStatus Status { get; set; } = ((CashPaymentStatus[])Enum.GetValues(typeof(CashPaymentStatus)))[0];
        public Guid UserProfileId { get; set; }
        public Guid ReservationId { get; set; }
    }
}