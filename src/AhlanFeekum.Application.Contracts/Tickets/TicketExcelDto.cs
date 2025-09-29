using System;

namespace AhlanFeekum.Tickets
{
    public abstract class TicketExcelDtoBase
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsFixed { get; set; }
    }
}