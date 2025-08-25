using System;

namespace AhlanFeekum.PropertyMedias
{
    public abstract class PropertyMediaExcelDtoBase
    {
        public string Image { get; set; } = null!;
        public int Order { get; set; }
        public bool isActive { get; set; }
    }
}