using AhlanFeekum.CashPayments;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.CashPayments
{
    public abstract class CashPaymentUpdateDtoBase : IHasConcurrencyStamp
    {
        public long Amount { get; set; }
        [Required]
        public string Currency { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public string? Description { get; set; }
        public CashPaymentStatus Status { get; set; }
        public Guid UserProfileId { get; set; }
        public Guid ReservationId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}