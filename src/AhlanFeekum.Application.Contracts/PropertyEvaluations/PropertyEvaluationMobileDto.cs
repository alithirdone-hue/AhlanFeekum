using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace AhlanFeekum.PropertyEvaluations
{
    public  class PropertyEvaluationMobileDto 
    {
        public Guid Id { get; set; }
        public int Cleanliness { get; set; }
        public int PriceAndValue { get; set; }
        public int Location { get; set; }
        public int Accuracy { get; set; }
        public int Attitude { get; set; }
        public string? RatingComment { get; set; }
        public Guid UserProfileId { get; set; }
        public  string? UserProfileName { get; set; }

    }
}