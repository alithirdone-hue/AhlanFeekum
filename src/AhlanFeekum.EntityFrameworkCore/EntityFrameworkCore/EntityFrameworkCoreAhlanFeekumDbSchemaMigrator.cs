using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AhlanFeekum.Data;
using Volo.Abp.DependencyInjection;

namespace AhlanFeekum.EntityFrameworkCore;

public class EntityFrameworkCoreAhlanFeekumDbSchemaMigrator
    : IAhlanFeekumDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreAhlanFeekumDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the AhlanFeekumDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<AhlanFeekumDbContext>()
            .Database
            .MigrateAsync();
    }
}
