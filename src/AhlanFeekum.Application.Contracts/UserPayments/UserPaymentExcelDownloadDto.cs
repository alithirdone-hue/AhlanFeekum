using AhlanFeekum.UserPayments;
using Volo.Abp.Application.Dtos;
using System;

namespace AhlanFeekum.UserPayments
{
    public abstract class UserPaymentExcelDownloadDtoBase
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public long? AmountMin { get; set; }
        public long? AmountMax { get; set; }
        public string? Currency { get; set; }
        public string? Description { get; set; }
        public string? ReceiptEmail { get; set; }
        public long? AmountCapturableMin { get; set; }
        public long? AmountCapturableMax { get; set; }
        public long? AmountReceivedMin { get; set; }
        public long? AmountReceivedMax { get; set; }
        public string? ConfirmationMethod { get; set; }
        public UserPaymentStatus? Status { get; set; }
        public string? StripPaymentId { get; set; }
        public string? StripClientSecret { get; set; }
        public DateTime? CreatedMin { get; set; }
        public DateTime? CreatedMax { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public Guid? UserProfileId { get; set; }
        public Guid? ReservationId { get; set; }

        public UserPaymentExcelDownloadDtoBase()
        {

        }
    }
}