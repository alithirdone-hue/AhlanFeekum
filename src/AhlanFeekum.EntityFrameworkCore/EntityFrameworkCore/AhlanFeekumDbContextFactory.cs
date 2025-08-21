using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AhlanFeekum.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class AhlanFeekumDbContextFactory : IDesignTimeDbContextFactory<AhlanFeekumDbContext>
{
    public AhlanFeekumDbContext CreateDbContext(string[] args)
    {
        AhlanFeekumEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<AhlanFeekumDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new AhlanFeekumDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AhlanFeekum.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
