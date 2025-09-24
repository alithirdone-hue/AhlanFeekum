using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.Governorates
{
    public abstract class GovernorateCreateDtoBase
    {
        [Required]
        public string Title { get; set; } = null!;
        public Guid IconId { get; set; }
        [Required]
        public string iconExtension { get; set; } = null!;
        public int Order { get; set; }
        public bool IsActive { get; set; } = true;
    }
}