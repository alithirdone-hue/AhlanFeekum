using Volo.Abp.Modularity;

namespace AhlanFeekum;

public abstract class AhlanFeekumApplicationTestBase<TStartupModule> : AhlanFeekumTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
