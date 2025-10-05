using Volo.Abp.Application.Dtos;
using System;

namespace AhlanFeekum.UserNotifications
{
    public abstract class GetUserNotificationsInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? Title { get; set; }
        public string? Body { get; set; }
        public Guid? UserProfileId { get; set; }
        public Guid? SitePropertyId { get; set; }

        public GetUserNotificationsInputBase()
        {

        }
    }
}