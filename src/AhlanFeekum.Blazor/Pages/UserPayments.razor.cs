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
using AhlanFeekum.UserPayments;
using AhlanFeekum.Permissions;
using AhlanFeekum.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;

using AhlanFeekum.UserPayments;



namespace AhlanFeekum.Blazor.Pages
{
    public partial class UserPayments
    {
        
        
            
        
            
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        private IReadOnlyList<UserPaymentWithNavigationPropertiesDto> UserPaymentList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateUserPayment { get; set; }
        private bool CanEditUserPayment { get; set; }
        private bool CanDeleteUserPayment { get; set; }
        private UserPaymentCreateDto NewUserPayment { get; set; }
        private Validations NewUserPaymentValidations { get; set; } = new();
        private UserPaymentUpdateDto EditingUserPayment { get; set; }
        private Validations EditingUserPaymentValidations { get; set; } = new();
        private Guid EditingUserPaymentId { get; set; }
        private Modal CreateUserPaymentModal { get; set; } = new();
        private Modal EditUserPaymentModal { get; set; } = new();
        private GetUserPaymentsInput Filter { get; set; }
        private DataGridEntityActionsColumn<UserPaymentWithNavigationPropertiesDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "userPayment-create-tab";
        protected string SelectedEditTab = "userPayment-edit-tab";
        private UserPaymentWithNavigationPropertiesDto? SelectedUserPayment;
        private IReadOnlyList<LookupDto<Guid>> UserProfilesCollection { get; set; } = new List<LookupDto<Guid>>();
private IReadOnlyList<LookupDto<Guid>> ReservationsCollection { get; set; } = new List<LookupDto<Guid>>();

        
        
        
        
        private List<UserPaymentWithNavigationPropertiesDto> SelectedUserPayments { get; set; } = new();
        private bool AllUserPaymentsSelected { get; set; }
        
        public UserPayments()
        {
            NewUserPayment = new UserPaymentCreateDto();
            EditingUserPayment = new UserPaymentUpdateDto();
            Filter = new GetUserPaymentsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            UserPaymentList = new List<UserPaymentWithNavigationPropertiesDto>();
            
            
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetUserProfileCollectionLookupAsync();


            await GetReservationCollectionLookupAsync();


            
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
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["UserPayments"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewUserPayment"], async () =>
            {
                await OpenCreateUserPaymentModalAsync();
            }, IconName.Add, requiredPolicyName: AhlanFeekumPermissions.UserPayments.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateUserPayment = await AuthorizationService
                .IsGrantedAsync(AhlanFeekumPermissions.UserPayments.Create);
            CanEditUserPayment = await AuthorizationService
                            .IsGrantedAsync(AhlanFeekumPermissions.UserPayments.Edit);
            CanDeleteUserPayment = await AuthorizationService
                            .IsGrantedAsync(AhlanFeekumPermissions.UserPayments.Delete);
                            
                            
        }

        private async Task GetUserPaymentsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await UserPaymentsAppService.GetListAsync(Filter);
            UserPaymentList = result.Items;
            TotalCount = (int)result.TotalCount;
            
            await ClearSelection();
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetUserPaymentsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await UserPaymentsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("AhlanFeekum") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/user-payments/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&AmountMin={Filter.AmountMin}&AmountMax={Filter.AmountMax}&Currency={HttpUtility.UrlEncode(Filter.Currency)}&Description={HttpUtility.UrlEncode(Filter.Description)}&ReceiptEmail={HttpUtility.UrlEncode(Filter.ReceiptEmail)}&AmountCapturableMin={Filter.AmountCapturableMin}&AmountCapturableMax={Filter.AmountCapturableMax}&AmountReceivedMin={Filter.AmountReceivedMin}&AmountReceivedMax={Filter.AmountReceivedMax}&ConfirmationMethod={HttpUtility.UrlEncode(Filter.ConfirmationMethod)}&Status={Filter.Status}&StripPaymentId={HttpUtility.UrlEncode(Filter.StripPaymentId)}&StripClientSecret={HttpUtility.UrlEncode(Filter.StripClientSecret)}&Created={HttpUtility.UrlEncode(Filter.Created)}&UserProfileId={Filter.UserProfileId}&ReservationId={Filter.ReservationId}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<UserPaymentWithNavigationPropertiesDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetUserPaymentsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateUserPaymentModalAsync()
        {
            NewUserPayment = new UserPaymentCreateDto{
                
                UserProfileId = UserProfilesCollection.Select(i=>i.Id).FirstOrDefault(),
ReservationId = ReservationsCollection.Select(i=>i.Id).FirstOrDefault(),

            };

            SelectedCreateTab = "userPayment-create-tab";
            
            
            await NewUserPaymentValidations.ClearAll();
            await CreateUserPaymentModal.Show();
        }

        private async Task CloseCreateUserPaymentModalAsync()
        {
            NewUserPayment = new UserPaymentCreateDto{
                
                UserProfileId = UserProfilesCollection.Select(i=>i.Id).FirstOrDefault(),
ReservationId = ReservationsCollection.Select(i=>i.Id).FirstOrDefault(),

            };
            await CreateUserPaymentModal.Hide();
        }

        private async Task OpenEditUserPaymentModalAsync(UserPaymentWithNavigationPropertiesDto input)
        {
            SelectedEditTab = "userPayment-edit-tab";
            
            
            var userPayment = await UserPaymentsAppService.GetWithNavigationPropertiesAsync(input.UserPayment.Id);
            
            EditingUserPaymentId = userPayment.UserPayment.Id;
            EditingUserPayment = ObjectMapper.Map<UserPaymentDto, UserPaymentUpdateDto>(userPayment.UserPayment);
            
            await EditingUserPaymentValidations.ClearAll();
            await EditUserPaymentModal.Show();
        }

        private async Task DeleteUserPaymentAsync(UserPaymentWithNavigationPropertiesDto input)
        {
            await UserPaymentsAppService.DeleteAsync(input.UserPayment.Id);
            await GetUserPaymentsAsync();
        }

        private async Task CreateUserPaymentAsync()
        {
            try
            {
                if (await NewUserPaymentValidations.ValidateAll() == false)
                {
                    return;
                }

                await UserPaymentsAppService.CreateAsync(NewUserPayment);
                await GetUserPaymentsAsync();
                await CloseCreateUserPaymentModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditUserPaymentModalAsync()
        {
            await EditUserPaymentModal.Hide();
        }

        private async Task UpdateUserPaymentAsync()
        {
            try
            {
                if (await EditingUserPaymentValidations.ValidateAll() == false)
                {
                    return;
                }

                await UserPaymentsAppService.UpdateAsync(EditingUserPaymentId, EditingUserPayment);
                await GetUserPaymentsAsync();
                await EditUserPaymentModal.Hide();                
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









        protected virtual async Task OnAmountMinChangedAsync(long? amountMin)
        {
            Filter.AmountMin = amountMin;
            await SearchAsync();
        }
        protected virtual async Task OnAmountMaxChangedAsync(long? amountMax)
        {
            Filter.AmountMax = amountMax;
            await SearchAsync();
        }
        protected virtual async Task OnCurrencyChangedAsync(string? currency)
        {
            Filter.Currency = currency;
            await SearchAsync();
        }
        protected virtual async Task OnDescriptionChangedAsync(string? description)
        {
            Filter.Description = description;
            await SearchAsync();
        }
        protected virtual async Task OnReceiptEmailChangedAsync(string? receiptEmail)
        {
            Filter.ReceiptEmail = receiptEmail;
            await SearchAsync();
        }
        protected virtual async Task OnAmountCapturableMinChangedAsync(long? amountCapturableMin)
        {
            Filter.AmountCapturableMin = amountCapturableMin;
            await SearchAsync();
        }
        protected virtual async Task OnAmountCapturableMaxChangedAsync(long? amountCapturableMax)
        {
            Filter.AmountCapturableMax = amountCapturableMax;
            await SearchAsync();
        }
        protected virtual async Task OnAmountReceivedMinChangedAsync(long? amountReceivedMin)
        {
            Filter.AmountReceivedMin = amountReceivedMin;
            await SearchAsync();
        }
        protected virtual async Task OnAmountReceivedMaxChangedAsync(long? amountReceivedMax)
        {
            Filter.AmountReceivedMax = amountReceivedMax;
            await SearchAsync();
        }
        protected virtual async Task OnConfirmationMethodChangedAsync(string? confirmationMethod)
        {
            Filter.ConfirmationMethod = confirmationMethod;
            await SearchAsync();
        }
        protected virtual async Task OnStatusChangedAsync(UserPaymentStatus? status)
        {
            Filter.Status = status;
            await SearchAsync();
        }
        protected virtual async Task OnStripPaymentIdChangedAsync(string? stripPaymentId)
        {
            Filter.StripPaymentId = stripPaymentId;
            await SearchAsync();
        }
        protected virtual async Task OnStripClientSecretChangedAsync(string? stripClientSecret)
        {
            Filter.StripClientSecret = stripClientSecret;
            await SearchAsync();
        }
        protected virtual async Task OnCreatedChangedAsync(string? created)
        {
            Filter.Created = created;
            await SearchAsync();
        }
        protected virtual async Task OnUserProfileIdChangedAsync(Guid? userProfileId)
        {
            Filter.UserProfileId = userProfileId;
            await SearchAsync();
        }
        protected virtual async Task OnReservationIdChangedAsync(Guid? reservationId)
        {
            Filter.ReservationId = reservationId;
            await SearchAsync();
        }
        

        private async Task GetUserProfileCollectionLookupAsync(string? newValue = null)
        {
            UserProfilesCollection = (await UserPaymentsAppService.GetUserProfileLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }

        private async Task GetReservationCollectionLookupAsync(string? newValue = null)
        {
            ReservationsCollection = (await UserPaymentsAppService.GetReservationLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }





        private Task SelectAllItems()
        {
            AllUserPaymentsSelected = true;
            
            return Task.CompletedTask;
        }

        private Task ClearSelection()
        {
            AllUserPaymentsSelected = false;
            SelectedUserPayments.Clear();
            
            return Task.CompletedTask;
        }

        private Task SelectedUserPaymentRowsChanged()
        {
            if (SelectedUserPayments.Count != PageSize)
            {
                AllUserPaymentsSelected = false;
            }
            
            return Task.CompletedTask;
        }

        private async Task DeleteSelectedUserPaymentsAsync()
        {
            var message = AllUserPaymentsSelected ? L["DeleteAllRecords"].Value : L["DeleteSelectedRecords", SelectedUserPayments.Count].Value;
            
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            if (AllUserPaymentsSelected)
            {
                await UserPaymentsAppService.DeleteAllAsync(Filter);
            }
            else
            {
                await UserPaymentsAppService.DeleteByIdsAsync(SelectedUserPayments.Select(x => x.UserPayment.Id).ToList());
            }

            SelectedUserPayments.Clear();
            AllUserPaymentsSelected = false;

            await GetUserPaymentsAsync();
        }


    }
}
