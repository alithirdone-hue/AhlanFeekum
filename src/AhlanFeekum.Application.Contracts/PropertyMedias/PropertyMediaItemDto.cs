using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AhlanFeekum.PropertyMedias
{
    public  class PropertyMediaItemDto
    {
        [Required]
        public Guid PropertyId { get; set; }
        [Required]
        public IFormFile Image { get; set; } = null!;
        public int Order { get; set; }
        public bool isActive { get; set; } = true;
    }
}