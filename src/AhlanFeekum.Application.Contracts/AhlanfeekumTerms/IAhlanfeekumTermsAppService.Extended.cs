using System;
using System.Threading.Tasks;

namespace AhlanFeekum.AhlanfeekumTerms
{
    public partial interface IAhlanfeekumTermsAppService
    {
        //Write your custom code here...

        Task<AhlanfeekumTermMobileDto> GetAsync();
    }
}