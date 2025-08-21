using AhlanFeekum.Samples;
using Xunit;

namespace AhlanFeekum.EntityFrameworkCore.Domains;

[Collection(AhlanFeekumTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<AhlanFeekumEntityFrameworkCoreTestModule>
{

}
