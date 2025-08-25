using System;

namespace AhlanFeekum.VerificationCodes;

public abstract class VerificationCodeDownloadTokenCacheItemBase
{
    public string Token { get; set; } = null!;
}