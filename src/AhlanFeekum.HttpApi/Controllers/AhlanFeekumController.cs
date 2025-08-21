using AhlanFeekum.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace AhlanFeekum.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AhlanFeekumController : AbpControllerBase
{
    protected AhlanFeekumController()
    {
        LocalizationResource = typeof(AhlanFeekumResource);
    }
}
