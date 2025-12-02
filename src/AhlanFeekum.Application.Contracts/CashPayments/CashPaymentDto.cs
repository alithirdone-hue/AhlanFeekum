using AhlanFeekum.CashPayments;
using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.CashPayments
{
    public abstract class CashPaymentDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public long Amount { get; set; }
        public string Currency { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public string? Description { get; set; }
        public CashPaymentStatus Status { get; set; }
        public Guid UserProfileId { get; set; }
        public Guid ReservationId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}