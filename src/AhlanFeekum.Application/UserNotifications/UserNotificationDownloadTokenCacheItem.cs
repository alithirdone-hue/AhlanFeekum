using System;

namespace AhlanFeekum.UserNotifications;

public abstract class UserNotificationDownloadTokenCacheItemBase
{
    public string Token { get; set; } = null!;
}