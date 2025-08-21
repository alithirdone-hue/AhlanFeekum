using Volo.Abp.Settings;

namespace AhlanFeekum.Settings;

public class AhlanFeekumSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AhlanFeekumSettings.MySetting1));
    }
}
