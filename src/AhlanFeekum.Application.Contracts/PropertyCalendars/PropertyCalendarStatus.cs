using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.PropertyCalendars
{
    public  class PropertyCalendarStatus
    {
        public Guid Id { get; set; }
       
        public DateOnly Date { get; set; }
        public bool IsAvailable { get; set; } = false;

        public string Status { get; set; }
        public Guid PropertyId { get; set; }
    }

   
}