using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.PropertyCalendars
{
    public  class PropertyCalendarItemDto
    {
        public Guid PropertyId { get; set; }
        public DateOnly Date { get; set; }
        public bool IsAvailable { get; set; } = false;
        public float? Price { get; set; }
        public string? Note { get; set; }
    }

   
}