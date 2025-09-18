using AhlanFeekum.Governorates;
using AhlanFeekum.PropertyEvaluations;
using AhlanFeekum.PropertyFeatures;
using AhlanFeekum.PropertyMedias;
using AhlanFeekum.PropertyTypes;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace AhlanFeekum.SiteProperties
{
    public  class SitePropertyWithDetails 
    {
        public SiteProperty SiteProperty { get; set; } = null!;
        public bool IsFavorite { get; set; } = false;
        public double? AverageRating { get; set; } = null;
        public PropertyAverageEvaluation? propertyAverageEvaluation { get; set; } = null;
        public PropertyType? PropertyType { get; set; } = null!;
        public Governorate ?Governorate { get; set; } = null!;


        public List<PropertyFeature> PropertyFeatures { get; set; } = null!;
        public PropertyMedia? MainImage { get; set; } = null;
        public List<PropertyMedia> Medias { get; set; }

   
    }
}