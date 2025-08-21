using Volo.Abp.Modularity;

namespace AhlanFeekum;

[DependsOn(
    typeof(AhlanFeekumDomainModule),
    typeof(AhlanFeekumTestBaseModule)
)]
public class AhlanFeekumDomainTestModule : AbpModule
{

}
