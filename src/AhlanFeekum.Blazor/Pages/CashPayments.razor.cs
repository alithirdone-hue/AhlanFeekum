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
using AhlanFeekum.CashPayments;
using AhlanFeekum.Permissions;
using AhlanFeekum.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;

using AhlanFeekum.CashPayments;



namespace AhlanFeekum.Blazor.Pages
{
    public partial class CashPayments
    {
        
        
            
        
            
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        private IReadOnlyList<CashPaymentWithNavigationPropertiesDto> CashPaymentList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateCashPayment { get; set; }
        private bool CanEditCashPayment { get; set; }
        private bool CanDeleteCashPayment { get; set; }
        private CashPaymentCreateDto NewCashPayment { get; set; }
        private Validations NewCashPaymentValidations { get; set; } = new();
        private CashPaymentUpdateDto EditingCashPayment { get; set; }
        private Validations EditingCashPaymentValidations { get; set; } = new();
        private Guid EditingCashPaymentId { get; set; }
        private Modal CreateCashPaymentModal { get; set; } = new();
        private Modal EditCashPaymentModal { get; set; } = new();
        private GetCashPaymentsInput Filter { get; set; }
        private DataGridEntityActionsColumn<CashPaymentWithNavigationPropertiesDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "cashPayment-create-tab";
        protected string SelectedEditTab = "cashPayment-edit-tab";
        private CashPaymentWithNavigationPropertiesDto? SelectedCashPayment;
        private IReadOnlyList<LookupDto<Guid>> UserProfilesCollection { get; set; } = new List<LookupDto<Guid>>();
private IReadOnlyList<LookupDto<Guid>> ReservationsCollection { get; set; } = new List<LookupDto<Guid>>();

        
        
        
        
        private List<CashPaymentWithNavigationPropertiesDto> SelectedCashPayments { get; set; } = new();
        private bool AllCashPaymentsSelected { get; set; }
        
        public CashPayments()
        {
            NewCashPayment = new CashPaymentCreateDto();
            EditingCashPayment = new CashPaymentUpdateDto();
            Filter = new GetCashPaymentsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            CashPaymentList = new List<CashPaymentWithNavigationPropertiesDto>();
            
            
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
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["CashPayments"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewCashPayment"], async () =>
            {
                await OpenCreateCashPaymentModalAsync();
            }, IconName.Add, requiredPolicyName: AhlanFeekumPermissions.CashPayments.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateCashPayment = await AuthorizationService
                .IsGrantedAsync(AhlanFeekumPermissions.CashPayments.Create);
            CanEditCashPayment = await AuthorizationService
                            .IsGrantedAsync(AhlanFeekumPermissions.CashPayments.Edit);
            CanDeleteCashPayment = await AuthorizationService
                            .IsGrantedAsync(AhlanFeekumPermissions.CashPayments.Delete);
                            
                            
        }

        private async Task GetCashPaymentsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await CashPaymentsAppService.GetListAsync(Filter);
            CashPaymentList = result.Items;
            TotalCount = (int)result.TotalCount;
            
            await ClearSelection();
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetCashPaymentsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await CashPaymentsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("AhlanFeekum") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/cash-payments/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&AmountMin={Filter.AmountMin}&AmountMax={Filter.AmountMax}&Currency={HttpUtility.UrlEncode(Filter.Currency)}&PaymentDateMin={Filter.PaymentDateMin?.ToString("O")}&PaymentDateMax={Filter.PaymentDateMax?.ToString("O")}&Description={HttpUtility.UrlEncode(Filter.Description)}&Status={Filter.Status}&UserProfileId={Filter.UserProfileId}&ReservationId={Filter.ReservationId}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<CashPaymentWithNavigationPropertiesDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetCashPaymentsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateCashPaymentModalAsync()
        {
            NewCashPayment = new CashPaymentCreateDto{
                PaymentDate = DateTime.Now,

                UserProfileId = UserProfilesCollection.Select(i=>i.Id).FirstOrDefault(),
ReservationId = ReservationsCollection.Select(i=>i.Id).FirstOrDefault(),

            };

            SelectedCreateTab = "cashPayment-create-tab";
            
            
            await NewCashPaymentValidations.ClearAll();
            await CreateCashPaymentModal.Show();
        }

        private async Task CloseCreateCashPaymentModalAsync()
        {
            NewCashPayment = new CashPaymentCreateDto{
                PaymentDate = DateTime.Now,

                UserProfileId = UserProfilesCollection.Select(i=>i.Id).FirstOrDefault(),
ReservationId = ReservationsCollection.Select(i=>i.Id).FirstOrDefault(),

            };
            await CreateCashPaymentModal.Hide();
        }

        private async Task OpenEditCashPaymentModalAsync(CashPaymentWithNavigationPropertiesDto input)
        {
            SelectedEditTab = "cashPayment-edit-tab";
            
            
            var cashPayment = await CashPaymentsAppService.GetWithNavigationPropertiesAsync(input.CashPayment.Id);
            
            EditingCashPaymentId = cashPayment.CashPayment.Id;
            EditingCashPayment = ObjectMapper.Map<CashPaymentDto, CashPaymentUpdateDto>(cashPayment.CashPayment);
            
            await EditingCashPaymentValidations.ClearAll();
            await EditCashPaymentModal.Show();
        }

        private async Task DeleteCashPaymentAsync(CashPaymentWithNavigationPropertiesDto input)
        {
            await CashPaymentsAppService.DeleteAsync(input.CashPayment.Id);
            await GetCashPaymentsAsync();
        }

        private async Task CreateCashPaymentAsync()
        {
            try
            {
                if (await NewCashPaymentValidations.ValidateAll() == false)
                {
                    return;
                }

                await CashPaymentsAppService.CreateAsync(NewCashPayment);
                await GetCashPaymentsAsync();
                await CloseCreateCashPaymentModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditCashPaymentModalAsync()
        {
            await EditCashPaymentModal.Hide();
        }

        private async Task UpdateCashPaymentAsync()
        {
            try
            {
                if (await EditingCashPaymentValidations.ValidateAll() == false)
                {
                    return;
                }

                await CashPaymentsAppService.UpdateAsync(EditingCashPaymentId, EditingCashPayment);
                await GetCashPaymentsAsync();
                await EditCashPaymentModal.Hide();                
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
        protected virtual async Task OnPaymentDateMinChangedAsync(DateTime? paymentDateMin)
        {
            Filter.PaymentDateMin = paymentDateMin.HasValue ? paymentDateMin.Value.Date : paymentDateMin;
            await SearchAsync();
        }
        protected virtual async Task OnPaymentDateMaxChangedAsync(DateTime? paymentDateMax)
        {
            Filter.PaymentDateMax = paymentDateMax.HasValue ? paymentDateMax.Value.Date.AddDays(1).AddSeconds(-1) : paymentDateMax;
            await SearchAsync();
        }
        protected virtual async Task OnDescriptionChangedAsync(string? description)
        {
            Filter.Description = description;
            await SearchAsync();
        }
        protected virtual async Task OnStatusChangedAsync(CashPaymentStatus? status)
        {
            Filter.Status = status;
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
            UserProfilesCollection = (await CashPaymentsAppService.GetUserProfileLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }

        private async Task GetReservationCollectionLookupAsync(string? newValue = null)
        {
            ReservationsCollection = (await CashPaymentsAppService.GetReservationLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }





        private Task SelectAllItems()
        {
            AllCashPaymentsSelected = true;
            
            return Task.CompletedTask;
        }

        private Task ClearSelection()
        {
            AllCashPaymentsSelected = false;
            SelectedCashPayments.Clear();
            
            return Task.CompletedTask;
        }

        private Task SelectedCashPaymentRowsChanged()
        {
            if (SelectedCashPayments.Count != PageSize)
            {
                AllCashPaymentsSelected = false;
            }
            
            return Task.CompletedTask;
        }

        private async Task DeleteSelectedCashPaymentsAsync()
        {
            var message = AllCashPaymentsSelected ? L["DeleteAllRecords"].Value : L["DeleteSelectedRecords", SelectedCashPayments.Count].Value;
            
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            if (AllCashPaymentsSelected)
            {
                await CashPaymentsAppService.DeleteAllAsync(Filter);
            }
            else
            {
                await CashPaymentsAppService.DeleteByIdsAsync(SelectedCashPayments.Select(x => x.CashPayment.Id).ToList());
            }

            SelectedCashPayments.Clear();
            AllCashPaymentsSelected = false;

            await GetCashPaymentsAsync();
        }


    }
}
