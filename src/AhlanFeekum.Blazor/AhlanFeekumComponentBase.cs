using AhlanFeekum.Localization;
using Volo.Abp.AspNetCore.Components;

namespace AhlanFeekum.Blazor;

public abstract class AhlanFeekumComponentBase : AbpComponentBase
{
    protected AhlanFeekumComponentBase()
    {
        LocalizationResource = typeof(AhlanFeekumResource);
    }
}
