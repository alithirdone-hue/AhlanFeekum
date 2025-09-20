using System;

namespace AhlanFeekum.Statuses
{
    public abstract class StatusExcelDtoBase
    {
        public string Name { get; set; } = null!;
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
}