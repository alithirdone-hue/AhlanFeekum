using AhlanFeekum.CashPayments;
using Volo.Abp.Application.Dtos;
using System;

namespace AhlanFeekum.CashPayments
{
    public abstract class CashPaymentExcelDownloadDtoBase
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public long? AmountMin { get; set; }
        public long? AmountMax { get; set; }
        public string? Currency { get; set; }
        public DateTime? PaymentDateMin { get; set; }
        public DateTime? PaymentDateMax { get; set; }
        public string? Description { get; set; }
        public CashPaymentStatus? Status { get; set; }
        public Guid? UserProfileId { get; set; }
        public Guid? ReservationId { get; set; }

        public CashPaymentExcelDownloadDtoBase()
        {

        }
    }
}