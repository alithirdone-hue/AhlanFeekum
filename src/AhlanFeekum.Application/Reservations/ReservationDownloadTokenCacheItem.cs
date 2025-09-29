using System;

namespace AhlanFeekum.Reservations;

public abstract class ReservationDownloadTokenCacheItemBase
{
    public string Token { get; set; } = null!;
}