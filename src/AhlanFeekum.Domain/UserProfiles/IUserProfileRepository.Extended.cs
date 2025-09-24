using AhlanFeekum.SiteProperties;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AhlanFeekum.UserProfiles
{
    public partial interface IUserProfileRepository
    {
        Task<HomePage> GetHomePageAsync(Guid? userId);

        Task<UserProfileWithDetails> GetWithDetailsAsync(Guid userId);
    }
}