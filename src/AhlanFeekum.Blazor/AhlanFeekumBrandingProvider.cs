using Microsoft.Extensions.Localization;
using AhlanFeekum.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace AhlanFeekum.Blazor;

[Dependency(ReplaceServices = true)]
public class AhlanFeekumBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<AhlanFeekumResource> _localizer;

    public AhlanFeekumBrandingProvider(IStringLocalizer<AhlanFeekumResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
