using Volo.Abp.Application.Dtos;
using System;

namespace AhlanFeekum.OnlyForYouSections
{
    public abstract class GetOnlyForYouSectionsInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? FirstPhotoExtension { get; set; }
        public string? SecondPhotoExtension { get; set; }
        public string? ThirdPhotoExtension { get; set; }

        public GetOnlyForYouSectionsInputBase()
        {

        }
    }
}