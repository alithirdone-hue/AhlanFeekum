using AhlanFeekum.UserProfiles;
using AhlanFeekum.SiteProperties;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace AhlanFeekum.PropertyEvaluations
{
    public class PropertyAverageEvaluation
    {
        public int CleanlinessAvg { get; set; } = 0;
        public int PriceAndValueAvg { get; set; } = 0;
        public int LocationAvg { get; set; } = 0;
        public int AccuracyAvg { get; set; } = 0;
        public int AttitudeAvg { get; set; } = 0;
    }
}