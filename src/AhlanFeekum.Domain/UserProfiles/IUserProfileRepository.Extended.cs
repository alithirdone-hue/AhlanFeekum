using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AhlanFeekum.UserProfiles
{
    public partial interface IUserProfileRepository
    {
        Task<HomePage> GetHomePageAsync(Guid? userId);
    }
}