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
using AhlanFeekum.UserNotifications;
using AhlanFeekum.Permissions;
using AhlanFeekum.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;



namespace AhlanFeekum.Blazor.Pages
{
    public partial class UserNotifications
    {
        
        
            
        
            
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        private IReadOnlyList<UserNotificationWithNavigationPropertiesDto> UserNotificationList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateUserNotification { get; set; }
        private bool CanEditUserNotification { get; set; }
        private bool CanDeleteUserNotification { get; set; }
        private UserNotificationCreateDto NewUserNotification { get; set; }
        private Validations NewUserNotificationValidations { get; set; } = new();
        private UserNotificationUpdateDto EditingUserNotification { get; set; }
        private Validations EditingUserNotificationValidations { get; set; } = new();
        private Guid EditingUserNotificationId { get; set; }
        private Modal CreateUserNotificationModal { get; set; } = new();
        private Modal EditUserNotificationModal { get; set; } = new();
        private GetUserNotificationsInput Filter { get; set; }
        private DataGridEntityActionsColumn<UserNotificationWithNavigationPropertiesDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "userNotification-create-tab";
        protected string SelectedEditTab = "userNotification-edit-tab";
        private UserNotificationWithNavigationPropertiesDto? SelectedUserNotification;
        private IReadOnlyList<LookupDto<Guid>> UserProfiles { get; set; } = new List<LookupDto<Guid>>();
        
        private string SelectedUserProfileId { get; set; }
        
        private string SelectedUserProfileText { get; set; }

        private Blazorise.Components.Autocomplete<LookupDto<Guid>, string> SelectedUserProfileAutoCompleteRef { get; set; } = new();

        private List<LookupDto<Guid>> SelectedUserProfiles { get; set; } = new List<LookupDto<Guid>>();private IReadOnlyList<LookupDto<Guid>> SiteProperties { get; set; } = new List<LookupDto<Guid>>();
        
        private string SelectedSitePropertyId { get; set; }
        
        private string SelectedSitePropertyText { get; set; }

        private Blazorise.Components.Autocomplete<LookupDto<Guid>, string> SelectedSitePropertyAutoCompleteRef { get; set; } = new();

        private List<LookupDto<Guid>> SelectedSiteProperties { get; set; } = new List<LookupDto<Guid>>();
        
        
        
        
        private List<UserNotificationWithNavigationPropertiesDto> SelectedUserNotifications { get; set; } = new();
        private bool AllUserNotificationsSelected { get; set; }
        
        public UserNotifications()
        {
            NewUserNotification = new UserNotificationCreateDto();
            EditingUserNotification = new UserNotificationUpdateDto();
            Filter = new GetUserNotificationsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            UserNotificationList = new List<UserNotificationWithNavigationPropertiesDto>();
            
            
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetUserProfileLookupAsync();


            await GetSitePropertyLookupAsync();


            
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
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["UserNotifications"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewUserNotification"], async () =>
            {
                await OpenCreateUserNotificationModalAsync();
            }, IconName.Add, requiredPolicyName: AhlanFeekumPermissions.UserNotifications.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateUserNotification = await AuthorizationService
                .IsGrantedAsync(AhlanFeekumPermissions.UserNotifications.Create);
            CanEditUserNotification = await AuthorizationService
                            .IsGrantedAsync(AhlanFeekumPermissions.UserNotifications.Edit);
            CanDeleteUserNotification = await AuthorizationService
                            .IsGrantedAsync(AhlanFeekumPermissions.UserNotifications.Delete);
                            
                            
        }

        private async Task GetUserNotificationsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await UserNotificationsAppService.GetListAsync(Filter);
            UserNotificationList = result.Items;
            TotalCount = (int)result.TotalCount;
            
            await ClearSelection();
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetUserNotificationsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await UserNotificationsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("AhlanFeekum") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/user-notifications/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&Title={HttpUtility.UrlEncode(Filter.Title)}&Body={HttpUtility.UrlEncode(Filter.Body)}&UserProfileId={Filter.UserProfileId}&SitePropertyId={Filter.SitePropertyId}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<UserNotificationWithNavigationPropertiesDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetUserNotificationsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateUserNotificationModalAsync()
        {
            SelectedUserProfiles = new List<LookupDto<Guid>>();
            SelectedUserProfileId = string.Empty;
            SelectedUserProfileText = string.Empty;

            await SelectedUserProfileAutoCompleteRef.Clear();

            SelectedSiteProperties = new List<LookupDto<Guid>>();
            SelectedSitePropertyId = string.Empty;
            SelectedSitePropertyText = string.Empty;

            await SelectedSitePropertyAutoCompleteRef.Clear();

            NewUserNotification = new UserNotificationCreateDto{
                
                
            };

            SelectedCreateTab = "userNotification-create-tab";
            
            
            await NewUserNotificationValidations.ClearAll();
            await CreateUserNotificationModal.Show();
        }

        private async Task CloseCreateUserNotificationModalAsync()
        {
            NewUserNotification = new UserNotificationCreateDto{
                
                
            };
            await CreateUserNotificationModal.Hide();
        }

        private async Task OpenEditUserNotificationModalAsync(UserNotificationWithNavigationPropertiesDto input)
        {
            SelectedEditTab = "userNotification-edit-tab";
            
            
            var userNotification = await UserNotificationsAppService.GetWithNavigationPropertiesAsync(input.UserNotification.Id);
            
            EditingUserNotificationId = userNotification.UserNotification.Id;
            EditingUserNotification = ObjectMapper.Map<UserNotificationDto, UserNotificationUpdateDto>(userNotification.UserNotification);
            SelectedUserProfiles = userNotification.UserProfiles.Select(a => new LookupDto<Guid>{ Id = a.Id, DisplayName = a.Name}).ToList();

            SelectedSiteProperties = userNotification.SiteProperties.Select(a => new LookupDto<Guid>{ Id = a.Id, DisplayName = a.PropertyTitle}).ToList();

            
            await EditingUserNotificationValidations.ClearAll();
            await EditUserNotificationModal.Show();
        }

        private async Task DeleteUserNotificationAsync(UserNotificationWithNavigationPropertiesDto input)
        {
            await UserNotificationsAppService.DeleteAsync(input.UserNotification.Id);
            await GetUserNotificationsAsync();
        }

        private async Task CreateUserNotificationAsync()
        {
            try
            {
                if (await NewUserNotificationValidations.ValidateAll() == false)
                {
                    return;
                }
                NewUserNotification.UserProfileIds = SelectedUserProfiles.Select(x => x.Id).ToList();

                NewUserNotification.SitePropertyIds = SelectedSiteProperties.Select(x => x.Id).ToList();


                await UserNotificationsAppService.CreateAsync(NewUserNotification);
                await GetUserNotificationsAsync();
                await CloseCreateUserNotificationModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditUserNotificationModalAsync()
        {
            await EditUserNotificationModal.Hide();
        }

        private async Task UpdateUserNotificationAsync()
        {
            try
            {
                if (await EditingUserNotificationValidations.ValidateAll() == false)
                {
                    return;
                }
                EditingUserNotification.UserProfileIds = SelectedUserProfiles.Select(x => x.Id).ToList();

                EditingUserNotification.SitePropertyIds = SelectedSiteProperties.Select(x => x.Id).ToList();


                await UserNotificationsAppService.UpdateAsync(EditingUserNotificationId, EditingUserNotification);
                await GetUserNotificationsAsync();
                await EditUserNotificationModal.Hide();                
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









        protected virtual async Task OnTitleChangedAsync(string? title)
        {
            Filter.Title = title;
            await SearchAsync();
        }
        protected virtual async Task OnBodyChangedAsync(string? body)
        {
            Filter.Body = body;
            await SearchAsync();
        }
        protected virtual async Task OnUserProfileIdChangedAsync(Guid? userProfileId)
        {
            Filter.UserProfileId = userProfileId;
            await SearchAsync();
        }
        protected virtual async Task OnSitePropertyIdChangedAsync(Guid? sitePropertyId)
        {
            Filter.SitePropertyId = sitePropertyId;
            await SearchAsync();
        }
        

        private async Task GetUserProfileLookupAsync(string? newValue = null)
        {
            UserProfiles = (await UserNotificationsAppService.GetUserProfileLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }

        private void AddUserProfile()
        {
            if (SelectedUserProfileId.IsNullOrEmpty())
            {
                return;
            }
            
            if (SelectedUserProfiles.Any(p => p.Id.ToString() == SelectedUserProfileId))
            {
                UiMessageService.Warn(L["ItemAlreadyAdded"]);
                return;
            }

            SelectedUserProfiles.Add(new LookupDto<Guid>
            {
                Id = Guid.Parse(SelectedUserProfileId),
                DisplayName = SelectedUserProfileText
            });
        }

        private async Task GetSitePropertyLookupAsync(string? newValue = null)
        {
            SiteProperties = (await UserNotificationsAppService.GetSitePropertyLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }

        private void AddSiteProperty()
        {
            if (SelectedSitePropertyId.IsNullOrEmpty())
            {
                return;
            }
            
            if (SelectedSiteProperties.Any(p => p.Id.ToString() == SelectedSitePropertyId))
            {
                UiMessageService.Warn(L["ItemAlreadyAdded"]);
                return;
            }

            SelectedSiteProperties.Add(new LookupDto<Guid>
            {
                Id = Guid.Parse(SelectedSitePropertyId),
                DisplayName = SelectedSitePropertyText
            });
        }





        private Task SelectAllItems()
        {
            AllUserNotificationsSelected = true;
            
            return Task.CompletedTask;
        }

        private Task ClearSelection()
        {
            AllUserNotificationsSelected = false;
            SelectedUserNotifications.Clear();
            
            return Task.CompletedTask;
        }

        private Task SelectedUserNotificationRowsChanged()
        {
            if (SelectedUserNotifications.Count != PageSize)
            {
                AllUserNotificationsSelected = false;
            }
            
            return Task.CompletedTask;
        }

        private async Task DeleteSelectedUserNotificationsAsync()
        {
            var message = AllUserNotificationsSelected ? L["DeleteAllRecords"].Value : L["DeleteSelectedRecords", SelectedUserNotifications.Count].Value;
            
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            if (AllUserNotificationsSelected)
            {
                await UserNotificationsAppService.DeleteAllAsync(Filter);
            }
            else
            {
                await UserNotificationsAppService.DeleteByIdsAsync(SelectedUserNotifications.Select(x => x.UserNotification.Id).ToList());
            }

            SelectedUserNotifications.Clear();
            AllUserNotificationsSelected = false;

            await GetUserNotificationsAsync();
        }

        private async Task SendAsync(UserNotificationWithNavigationPropertiesDto input)
        {
            SelectedEditTab = "userNotification-edit-tab";


            var userNotification = await UserNotificationsAppService.GetWithNavigationPropertiesAsync(input.UserNotification.Id);
            if (userNotification != null)
            {
                await UserNotificationsAppService.SendAsync(userNotification);
            }

        }
        }
    }
