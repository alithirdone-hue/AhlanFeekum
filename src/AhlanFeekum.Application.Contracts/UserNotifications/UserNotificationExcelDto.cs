using System;

namespace AhlanFeekum.UserNotifications
{
    public abstract class UserNotificationExcelDtoBase
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}