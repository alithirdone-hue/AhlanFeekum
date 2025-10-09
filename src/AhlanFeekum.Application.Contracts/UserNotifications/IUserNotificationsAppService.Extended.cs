using System;
using System.Threading.Tasks;

namespace AhlanFeekum.UserNotifications
{
    public partial interface IUserNotificationsAppService
    {
        //Write your custom code here...

        Task<bool> SendAsync(UserNotificationWithNavigationPropertiesDto input);
    }
}