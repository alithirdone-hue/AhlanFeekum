using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Web;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using AhlanFeekum.Statuses;
using AhlanFeekum.Permissions;
using AhlanFeekum.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;



namespace AhlanFeekum.Blazor.Pages
{
    public partial class Statuses
    {
        
        
            
        
            
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        private IReadOnlyList<StatusDto> StatusList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateStatus { get; set; }
        private bool CanEditStatus { get; set; }
        private bool CanDeleteStatus { get; set; }
        private StatusCreateDto NewStatus { get; set; }
        private Validations NewStatusValidations { get; set; } = new();
        private StatusUpdateDto EditingStatus { get; set; }
        private Validations EditingStatusValidations { get; set; } = new();
        private Guid EditingStatusId { get; set; }
        private Modal CreateStatusModal { get; set; } = new();
        private Modal EditStatusModal { get; set; } = new();
        private GetStatusesInput Filter { get; set; }
        private DataGridEntityActionsColumn<StatusDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "status-create-tab";
        protected string SelectedEditTab = "status-edit-tab";
        private StatusDto? SelectedStatus;
        
        
        
        
        
        private List<StatusDto> SelectedStatuses { get; set; } = new();
        private bool AllStatusesSelected { get; set; }
        
        public Statuses()
        {
            NewStatus = new StatusCreateDto();
            EditingStatus = new StatusUpdateDto();
            Filter = new GetStatusesInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            StatusList = new List<StatusDto>();
            
            
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                
                await SetBreadcrumbItemsAsync();
                await SetToolbarItemsAsync();
                await InvokeAsync(StateHasChanged);
            }
        }  

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Statuses"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewStatus"], async () =>
            {
                await OpenCreateStatusModalAsync();
            }, IconName.Add, requiredPolicyName: AhlanFeekumPermissions.Statuses.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateStatus = await AuthorizationService
                .IsGrantedAsync(AhlanFeekumPermissions.Statuses.Create);
            CanEditStatus = await AuthorizationService
                            .IsGrantedAsync(AhlanFeekumPermissions.Statuses.Edit);
            CanDeleteStatus = await AuthorizationService
                            .IsGrantedAsync(AhlanFeekumPermissions.Statuses.Delete);
                            
                            
        }

        private async Task GetStatusesAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await StatusesAppService.GetListAsync(Filter);
            StatusList = result.Items;
            TotalCount = (int)result.TotalCount;
            
            await ClearSelection();
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetStatusesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await StatusesAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("AhlanFeekum") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/statuses/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&Name={HttpUtility.UrlEncode(Filter.Name)}&OrderMin={Filter.OrderMin}&OrderMax={Filter.OrderMax}&IsActive={Filter.IsActive}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<StatusDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetStatusesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateStatusModalAsync()
        {
            NewStatus = new StatusCreateDto{
                
                
            };

            SelectedCreateTab = "status-create-tab";
            
            
            await NewStatusValidations.ClearAll();
            await CreateStatusModal.Show();
        }

        private async Task CloseCreateStatusModalAsync()
        {
            NewStatus = new StatusCreateDto{
                
                
            };
            await CreateStatusModal.Hide();
        }

        private async Task OpenEditStatusModalAsync(StatusDto input)
        {
            SelectedEditTab = "status-edit-tab";
            
            
            var status = await StatusesAppService.GetAsync(input.Id);
            
            EditingStatusId = status.Id;
            EditingStatus = ObjectMapper.Map<StatusDto, StatusUpdateDto>(status);
            
            await EditingStatusValidations.ClearAll();
            await EditStatusModal.Show();
        }

        private async Task DeleteStatusAsync(StatusDto input)
        {
            await StatusesAppService.DeleteAsync(input.Id);
            await GetStatusesAsync();
        }

        private async Task CreateStatusAsync()
        {
            try
            {
                if (await NewStatusValidations.ValidateAll() == false)
                {
                    return;
                }

                await StatusesAppService.CreateAsync(NewStatus);
                await GetStatusesAsync();
                await CloseCreateStatusModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditStatusModalAsync()
        {
            await EditStatusModal.Hide();
        }

        private async Task UpdateStatusAsync()
        {
            try
            {
                if (await EditingStatusValidations.ValidateAll() == false)
                {
                    return;
                }

                await StatusesAppService.UpdateAsync(EditingStatusId, EditingStatus);
                await GetStatusesAsync();
                await EditStatusModal.Hide();                
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name;
        }

        private void OnSelectedEditTabChanged(string name)
        {
            SelectedEditTab = name;
        }









        protected virtual async Task OnNameChangedAsync(string? name)
        {
            Filter.Name = name;
            await SearchAsync();
        }
        protected virtual async Task OnOrderMinChangedAsync(int? orderMin)
        {
            Filter.OrderMin = orderMin;
            await SearchAsync();
        }
        protected virtual async Task OnOrderMaxChangedAsync(int? orderMax)
        {
            Filter.OrderMax = orderMax;
            await SearchAsync();
        }
        protected virtual async Task OnIsActiveChangedAsync(bool? isActive)
        {
            Filter.IsActive = isActive;
            await SearchAsync();
        }
        





        private Task SelectAllItems()
        {
            AllStatusesSelected = true;
            
            return Task.CompletedTask;
        }

        private Task ClearSelection()
        {
            AllStatusesSelected = false;
            SelectedStatuses.Clear();
            
            return Task.CompletedTask;
        }

        private Task SelectedStatusRowsChanged()
        {
            if (SelectedStatuses.Count != PageSize)
            {
                AllStatusesSelected = false;
            }
            
            return Task.CompletedTask;
        }

        private async Task DeleteSelectedStatusesAsync()
        {
            var message = AllStatusesSelected ? L["DeleteAllRecords"].Value : L["DeleteSelectedRecords", SelectedStatuses.Count].Value;
            
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            if (AllStatusesSelected)
            {
                await StatusesAppService.DeleteAllAsync(Filter);
            }
            else
            {
                await StatusesAppService.DeleteByIdsAsync(SelectedStatuses.Select(x => x.Id).ToList());
            }

            SelectedStatuses.Clear();
            AllStatusesSelected = false;

            await GetStatusesAsync();
        }


    }
}
