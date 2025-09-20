using Volo.Abp.Application.Dtos;
using System;

namespace AhlanFeekum.SpecialAdvertisments
{
    public abstract class SpecialAdvertismentExcelDownloadDtoBase
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? ImageExtension { get; set; }
        public int? OrderMin { get; set; }
        public int? OrderMax { get; set; }
        public bool? IsActive { get; set; }
        public Guid? SitePropertyId { get; set; }

        public SpecialAdvertismentExcelDownloadDtoBase()
        {

        }
    }
}