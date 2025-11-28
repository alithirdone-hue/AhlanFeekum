using AhlanFeekum.UserPayments;
using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.UserPayments
{
    public abstract class UserPaymentDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
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
        public string Created { get; set; } = null!;
        public Guid UserProfileId { get; set; }
        public Guid ReservationId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}