using System;
using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.SiteProperties
{
    public class SitePropertySetPriceDto
    {
        //Write your custom code here...
        [Required]
        public Guid PropertyId { get; set; }
        [Required]
        public int PricePerNight { get; set; }
    }
}