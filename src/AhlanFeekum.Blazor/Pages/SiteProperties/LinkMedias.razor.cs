using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.Guids;
using Volo.Abp.Application.Dtos;
using Microsoft.Extensions.Logging;
using AhlanFeekum.Permissions;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.PropertyMedias;
using AhlanFeekum.Helper;


namespace AhlanFeekum.Blazor.Pages.SiteProperties
{
    public partial class LinkMedias
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        public IUiMessageService uiMessageService { get; set; }

        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar { get; } = new PageToolbar();
        private bool CanEditSiteProperty { get; set; }

        [Inject]
        protected IGuidGenerator GuidGenerator { get; set; }

        private Guid EditingId { get; set; }
        private Validations EditingSitePropertyValidations { get; set; } = new();

        [Inject]
        public ISitePropertiesAppService SitePropertiesAppService { get; set; }

        [Inject]
        public IPropertyMediasAppService MediasAppService { get; set; }

        private List<PropertyMediaDto> MediaDtos { get; set; }
        private SitePropertyDto SitePropertyDto { get; set; }

        private string SelectedTab { get; set; }
        public LinkMedias()
        {

        }

        protected override async Task OnInitializedAsync()
        {
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();

            if (!Id.IsNullOrEmpty())
            {
                try
                {
                    EditingId = Guid.Parse(Id);

                    SitePropertyDto = await SitePropertiesAppService.GetAsync(EditingId);

                    GetPropertyMediasInput getMediasInput = new GetPropertyMediasInput();
                    getMediasInput.SkipCount = 0;
                    getMediasInput.MaxResultCount = 1000;
                    getMediasInput.SitePropertyId = EditingId;

                    PagedResultDto<PropertyMediaWithNavigationPropertiesDto> pagedResultDto = await MediasAppService.GetListAsync(getMediasInput);
                    if (pagedResultDto != null && pagedResultDto.TotalCount > 0)
                    {
                        MediaDtos = pagedResultDto.Items.Select(m => m.PropertyMedia).ToList();
                        SelectedTab = "1";
                    }

                    // await SetNewAsync();
                }
                catch (Exception ex)
                {
                    //await uiMessageService.Error("Error in get data");
                    Logger.LogError(ex, "Error in get data");
                    //NavigationManager.NavigateTo("/page-informations");
                    NavigationManager.NavigateTo($"/{NavigationManager.ToBaseRelativePath(NavigationManager.Uri).Split('/')?[0] ?? ""}");
                    //{NavigationManager.ToBaseRelativePath(NavigationManager.Uri).Split('/')?[0] ?? ""}
                    await HandleErrorAsync(ex);
                }
            }
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:SiteProperties"],
                url: $"{String.Join("/", NavigationManager.ToBaseRelativePath(NavigationManager.Uri).Split('/'), 0, 2) ?? ""}"));

            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["LinkCategories"]));
            return ValueTask.CompletedTask;
        }
        private async Task SetPermissionsAsync()
        {
            CanEditSiteProperty = await AuthorizationService
                            .IsGrantedAsync(AhlanFeekumPermissions.SiteProperties.Edit);

            if (!CanEditSiteProperty)
                throw new UnauthorizedAccessException();

        }

        protected async Task addNewMediaAsync()
        {
            var data = new PropertyMediaDto();
            data.Id = GuidGenerator.Create();
            if (!Id.IsNullOrEmpty())
                data.SitePropertyId = Guid.Parse(Id);
            data.isActive = true;
            if (MediaDtos.IsNullOrEmpty())
                MediaDtos = new List<PropertyMediaDto>();
            data.Order = MediaDtos.Count + 1;

            SelectedTab = (MediaDtos.Count + 1).ToString();
            MediaDtos.Add(data);
        }
        protected void MediaDeleted(PropertyMediaDto input)
        {
            MediaDtos.Remove(input);
            if (!MediaDtos.IsNullOrEmpty() && SelectedTab != null && SelectedTab.Trim() != "1")
                SelectedTab = (Int32.Parse(SelectedTab) - 1).ToString();
            else
                if (SelectedTab != null && SelectedTab.Trim() == "1")
                SelectedTab = "1";
        }

        private async Task UpdateMediasAsync()
        {
            try
            {
                if (await ValidateMedia() == false)
                {
                    return;
                }

                await MediasAppService.UpdateSitePropertyMediasAsync(EditingId, MediaDtos);
                await uiMessageService.Success(L["Message:SuccessfullyUpdated"]);
                NavigationManager.NavigateTo($"{String.Join("/", NavigationManager.ToBaseRelativePath(NavigationManager.Uri).Split('/'), 0, 2) ?? ""}");
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task<bool> ValidateMedia()
        {
            foreach (var (item, index) in MediaDtos.WithIndex())
            {
                if (item.Image.IsNullOrEmpty())
                {
                    await uiMessageService.Error($"{L["SitePropertyMediaImageValidation"]} ({item.Order})");
                    return false;
                }
            }
            return true;
        }
        private async Task Cancel()
        {
            var confirm = await uiMessageService.Confirm(L["ReturnBackConfirmationMessage"]);

            if (confirm)
            {
                NavigationManager.NavigateTo($"{String.Join("/", NavigationManager.ToBaseRelativePath(NavigationManager.Uri).Split('/'), 0, 2) ?? ""}");
            }
        }

        //private async Task SetNewAsync()
        //{
        //    GetMediaGalleriesInput getMediaGalleriesInput = new GetMediaGalleriesInput();
        //    getMediaGalleriesInput.MaxResultCount = 1;
        //    getMediaGalleriesInput.IsActive = true;
        //    getMediaGalleriesInput.IsForPagesOnly = true;

        //    PagedResultDto<SitePropertyDto> pagedResult = await MediaGalleriesAppService.GetListAsync(getMediaGalleriesInput);
        //    if (pagedResult != null)
        //    {
        //        EditingSiteProperty.Order = Convert.ToInt32(pagedResult.TotalCount);
        //    }
        //}

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedTab = name;
        }
    }
}