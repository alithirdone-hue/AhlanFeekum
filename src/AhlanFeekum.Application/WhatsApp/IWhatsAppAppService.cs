using AhlanFeekum.UserNotifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AhlanFeekum.FireBase
{
    public interface IWhatsAppAppService: IApplicationService
    {
        Task<bool> SendMessage(string message);
      //  Task<string> FireBaseDynamicLink(Guid grantId);
    }
}
