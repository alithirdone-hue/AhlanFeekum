using AhlanFeekum.CashPayments;
using System;

namespace AhlanFeekum.CashPayments
{
    public abstract class CashPaymentExcelDtoBase
    {
        public long Amount { get; set; }
        public string Currency { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public string? Description { get; set; }
        public CashPaymentStatus Status { get; set; }
    }
}