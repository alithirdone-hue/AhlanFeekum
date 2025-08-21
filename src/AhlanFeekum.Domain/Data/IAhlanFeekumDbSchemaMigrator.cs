using System.Threading.Tasks;

namespace AhlanFeekum.Data;

public interface IAhlanFeekumDbSchemaMigrator
{
    Task MigrateAsync();
}
