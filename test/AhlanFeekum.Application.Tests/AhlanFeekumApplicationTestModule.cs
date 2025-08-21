using Volo.Abp.Modularity;

namespace AhlanFeekum;

[DependsOn(
    typeof(AhlanFeekumApplicationModule),
    typeof(AhlanFeekumDomainTestModule)
)]
public class AhlanFeekumApplicationTestModule : AbpModule
{

}
