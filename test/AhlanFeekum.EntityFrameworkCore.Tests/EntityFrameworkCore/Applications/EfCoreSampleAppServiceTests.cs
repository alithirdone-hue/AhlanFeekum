using AhlanFeekum.Samples;
using Xunit;

namespace AhlanFeekum.EntityFrameworkCore.Applications;

[Collection(AhlanFeekumTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<AhlanFeekumEntityFrameworkCoreTestModule>
{

}
