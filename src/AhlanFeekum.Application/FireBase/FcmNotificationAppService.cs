using AhlanFeekum;
using AhlanFeekum.FireBase;
using AhlanFeekum.UserNotifications;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace AhlanFeekum.FireBase
{
    [RemoteService(false)]
    public class FcmNotificationsAppService : AhlanFeekumAppService, IFcmNotificationsAppService
    {
        private readonly ILogger<FcmNotificationsAppService> _logger;

        public FcmNotificationsAppService(ILogger<FcmNotificationsAppService> logger)
        {
            _logger = logger;
        }

        public async Task<string> SendNotification(UserNotificationWithNavigationPropertiesDto notificationModel)
        {

            foreach (var user in notificationModel.UserProfiles)
            {
                try
                {
                    string deviceToken = user.FcmToken;

                    if (!deviceToken.IsNullOrEmpty())
                    {

                        var message = new Message()
                        {
                            Token = deviceToken,
                            Notification = new Notification()
                            {
                                Title = notificationModel.UserNotification.Title,
                                Body = notificationModel.UserNotification.Body,
                            },
                            //    // Optional: add custom data
                            //    Data = new Dictionary<string, string>()
                            //{
                            //    { "Type",notificationModel.Data.Type },
                            //    { "Id",notificationModel.Data.Id },
                            //    { "ReferenceId", notificationModel.Data.ReferenceId},
                            //    { "IsAcknowledge", notificationModel.Data.IsAcknowledge.ToString()}

                            //}
                        };

                        string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "FCM push notification Error");
                    return null;

                }
            }
            return "DONE";
        }
    }
}
