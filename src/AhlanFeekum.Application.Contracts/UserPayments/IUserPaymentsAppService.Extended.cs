using AhlanFeekum.Shared;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace AhlanFeekum.UserPayments
{
    public partial interface IUserPaymentsAppService
    {
        //Write your custom code here...

        Task<PagedResultDto<LookupDto<int>>> GetPaymentMethodLookupAsync();
    }
}