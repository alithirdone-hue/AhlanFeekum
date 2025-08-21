using System;
using System.Collections.Generic;
using System.Text;
using AhlanFeekum.Localization;
using Volo.Abp.Application.Services;

namespace AhlanFeekum;

/* Inherit your application services from this class.
 */
public abstract class AhlanFeekumAppService : ApplicationService
{
    protected AhlanFeekumAppService()
    {
        LocalizationResource = typeof(AhlanFeekumResource);
    }
}
