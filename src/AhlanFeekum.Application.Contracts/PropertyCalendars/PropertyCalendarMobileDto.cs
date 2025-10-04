using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.PropertyCalendars
{
    public  class PropertyCalendarMobileDto
    {
        public Guid Id { get; set; }
       
        public DateOnly Date { get; set; }
        public bool IsAvailable { get; set; } = false;
        public Guid PropertyId { get; set; }
    }

   
}