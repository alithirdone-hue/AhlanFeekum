using AhlanFeekum.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AhlanFeekum.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AhlanFeekumEntityFrameworkCoreModule),
    typeof(AhlanFeekumApplicationContractsModule)
    )]
public class AhlanFeekumDbMigratorModule : AbpModule
{
}
