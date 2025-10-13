using AhlanFeekum.UserNotifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AhlanFeekum.FireBase
{
    public interface IFcmNotificationsAppService: IApplicationService
    {
        Task<string> SendNotification(UserNotificationWithNavigationPropertiesDto notificationModel);
      //  Task<string> FireBaseDynamicLink(Guid grantId);
    }
}
