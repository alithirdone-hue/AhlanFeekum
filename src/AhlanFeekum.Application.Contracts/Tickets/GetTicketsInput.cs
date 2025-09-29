using Volo.Abp.Application.Dtos;
using System;

namespace AhlanFeekum.Tickets
{
    public abstract class GetTicketsInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Description { get; set; }
        public bool? IsFixed { get; set; }
        public Guid? UserProfileId { get; set; }

        public GetTicketsInputBase()
        {

        }
    }
}