using System;

namespace AhlanFeekum.UserPayments;

public abstract class UserPaymentDownloadTokenCacheItemBase
{
    public string Token { get; set; } = null!;
}