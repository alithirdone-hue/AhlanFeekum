using System;

namespace AhlanFeekum.UserProfiles;

public abstract class UserProfileDownloadTokenCacheItemBase
{
    public string Token { get; set; } = null!;
    public string SecurityCode { get; set; }
}