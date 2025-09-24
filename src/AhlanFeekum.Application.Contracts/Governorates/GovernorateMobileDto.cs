using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.Governorates
{
    public  class GovernorateMobileDto 
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Icon { get; set; } = null!;

    }
}