using System;

namespace AhlanFeekum.Tickets;

public abstract class TicketDownloadTokenCacheItemBase
{
    public string Token { get; set; } = null!;
}