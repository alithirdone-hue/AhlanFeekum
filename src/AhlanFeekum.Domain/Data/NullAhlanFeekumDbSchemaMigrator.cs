using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace AhlanFeekum.Data;

/* This is used if database provider does't define
 * IAhlanFeekumDbSchemaMigrator implementation.
 */
public class NullAhlanFeekumDbSchemaMigrator : IAhlanFeekumDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
