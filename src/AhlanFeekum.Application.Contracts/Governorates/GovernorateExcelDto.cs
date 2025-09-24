using System;

namespace AhlanFeekum.Governorates
{
    public abstract class GovernorateExcelDtoBase
    {
        public string Title { get; set; } = null!;
        public string iconExtension { get; set; } = null!;
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
}