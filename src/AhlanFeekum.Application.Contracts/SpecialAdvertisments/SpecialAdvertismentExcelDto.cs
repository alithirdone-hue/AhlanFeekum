using System;

namespace AhlanFeekum.SpecialAdvertisments
{
    public abstract class SpecialAdvertismentExcelDtoBase
    {
        public string Image { get; set; } = null!;
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
}