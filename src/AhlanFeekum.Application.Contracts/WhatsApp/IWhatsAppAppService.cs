using AhlanFeekum.UserNotifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AhlanFeekum.WhatsApp
{
    public interface IWhatsAppAppService: IApplicationService
    {
        Task<bool> SendMessage(string message);
        Task<bool> SendMessage(string message, string phoneNumber);
        Task<bool> SendTemplateMessage(string sendTo, string templateName, string[] exampleArr, string mediaUri = null);
        Task<bool> SendTemplateMessage(string sendTo, string templateName, string[] exampleArr, string token, string mediaUri = null);
      //  Task<string> FireBaseDynamicLink(Guid grantId);
    }
}
