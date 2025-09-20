using Volo.Abp.Application.Dtos;
using System;

namespace AhlanFeekum.Statuses
{
    public abstract class GetStatusesInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? Name { get; set; }
        public int? OrderMin { get; set; }
        public int? OrderMax { get; set; }
        public bool? IsActive { get; set; }

        public GetStatusesInputBase()
        {

        }
    }
}