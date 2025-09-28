using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.PropertyEvaluations
{
    public class PropertyEvaluationCreateMobileDto
    {
        [Required]
        [Range(PropertyEvaluationConsts.CleanlinessMinLength, PropertyEvaluationConsts.CleanlinessMaxLength)]
        public int Cleanliness { get; set; }
        [Required]
        [Range(PropertyEvaluationConsts.PriceAndValueMinLength, PropertyEvaluationConsts.PriceAndValueMaxLength)]
        public int PriceAndValue { get; set; }
        [Required]
        [Range(PropertyEvaluationConsts.LocationMinLength, PropertyEvaluationConsts.LocationMaxLength)]
        public int Location { get; set; }
        [Required]
        [Range(PropertyEvaluationConsts.AccuracyMinLength, PropertyEvaluationConsts.AccuracyMaxLength)]
        public int Accuracy { get; set; }
        [Required]
        [Range(PropertyEvaluationConsts.AttitudeMinLength, PropertyEvaluationConsts.AttitudeMaxLength)]
        public int Attitude { get; set; }
        public string? RatingComment { get; set; }
        public Guid SitePropertyId { get; set; }
    }
}