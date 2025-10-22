using System;
using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.SiteProperties
{
    public class SitePropertyActiveDeActiveRequest
    {
        //Write your custom code here...
        [Required]
        public Guid PropertyId { get; set; }
        [Required]
        public bool isActive { get; set; }
    }
}