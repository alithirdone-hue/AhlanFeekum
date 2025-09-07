using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.OnlyForYouSections
{
    public  class OnlyForYouSectionMobileDto
    {
        public Guid Id { get; set; }
        public string FirstPhoto { get; set; } = null!;
        public string SecondPhoto { get; set; } = null!;
        public string ThirdPhoto { get; set; } = null!;

    }
}