using AhlanFeekum.MobileResponses;
using System.Threading.Tasks;

namespace AhlanFeekum.UserProfiles
{
    public partial interface IUserProfilesAppService
    {
        //Write your custom code here...
        Task<MobileResponseDto> RegisterAsync(RegisterCreateMobileDto input);
    }
}