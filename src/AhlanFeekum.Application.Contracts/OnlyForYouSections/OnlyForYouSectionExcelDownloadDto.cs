using Volo.Abp.Application.Dtos;
using System;

namespace AhlanFeekum.OnlyForYouSections
{
    public abstract class OnlyForYouSectionExcelDownloadDtoBase
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? FirstPhotoExtension { get; set; }
        public string? SecondPhotoExtension { get; set; }
        public string? ThirdPhotoExtension { get; set; }

        public OnlyForYouSectionExcelDownloadDtoBase()
        {

        }
    }
}