using Volo.Abp.Modularity;

namespace AhlanFeekum;

/* Inherit from this class for your domain layer tests. */
public abstract class AhlanFeekumDomainTestBase<TStartupModule> : AhlanFeekumTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
