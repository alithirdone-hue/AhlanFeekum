using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.Statuses
{
    public abstract class StatusCreateDtoBase
    {
        [Required]
        public string Name { get; set; } = null!;
        public int Order { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}