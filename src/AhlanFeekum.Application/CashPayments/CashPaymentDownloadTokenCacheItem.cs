using System;

namespace AhlanFeekum.CashPayments;

public abstract class CashPaymentDownloadTokenCacheItemBase
{
    public string Token { get; set; } = null!;
}