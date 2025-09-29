using System.Threading.Tasks;

namespace AhlanFeekum.Tickets
{
    public partial interface ITicketsAppService
    {
        //Write your custom code here...
        Task<TicketDto> CreateAsync(TicketCreateMobileDto input);
    }
}