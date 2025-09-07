using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;
using System.IO;
using Volo.Abp;
using Volo.Abp.Guids;
using Volo.Abp.BlobStoring;
using AhlanFeekum.PropertyMedias;

namespace AhlanFeekum.Blazor.Components
{
    public partial class CustomPropertyMedia
    {
        [Parameter]
        public PropertyMediaDto PropertyMedia { get; set; }

        [Parameter]
        public EventCallback<PropertyMediaDto> PropertyMediaDeleted { get; set; }

        [Inject]
        public IUiMessageService uiMessageService { get; set; }

        [Inject]
        public IBlobContainer<PropertyMediaContainer> PropertyMediaContainer { get; set; }

        [Inject]
        protected IGuidGenerator GuidGenerator { get; set; }
        private Validations PropertyMediaValidations { get; set; } = new();


        public CustomPropertyMedia()
        {

        }

        protected override async Task OnInitializedAsync()
        {

        }

        private async Task deletePropertyMediaAsync(PropertyMediaDto input)
        {
            var confirm = await uiMessageService.Confirm(L["DeleteConfirmationMessage"]);

            if (confirm)
            {
                await PropertyMediaDeleted.InvokeAsync(input);
            }
        }
        private async Task RemovePropertyMedia()
        {
            PropertyMedia.Image = null;
        }
        public async Task FileChanged(FileChangedEventArgs e)
        {
            try
            {
                if (e.Files.Count() == 0)
                {
                    PropertyMedia.Image = null;
                }

            }
            catch (UserFriendlyException ex)
            {
                await HandleErrorAsync(ex);
            }
        }
        public async Task OnFileUpload(FileUploadEventArgs e)
        {
            try
            {
                using (MemoryStream result = new MemoryStream())
                {
                    await e.File.OpenReadStream(long.MaxValue).CopyToAsync(result);
                    PropertyMedia.FileContent = await result.GetAllBytesAsync();
                    // PropertyMedia.File = $"{Path.GetFileNameWithoutExtension(e.File.Name)}_{GuidGenerator.Create().ToString("N")}{Path.GetExtension(e.File.Name)}";
                    PropertyMedia.Image = $"{GuidGenerator.Create().ToString("N")}{Path.GetExtension(e.File.Name)}";

                    PropertyMedia.FileNewUpload = true;
                }
            }
            catch (UserFriendlyException ex)
            {
                await HandleErrorAsync(ex);
            }
        }

    }
}