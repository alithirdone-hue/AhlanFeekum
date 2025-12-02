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
using AhlanFeekum.Reservations;
using AhlanFeekum.Permissions;
using AhlanFeekum.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;

using AhlanFeekum.Reservations;

using AhlanFeekum.Reservations;



namespace AhlanFeekum.Blazor.Pages
{
    public partial class Reservations
    {
        
        
            
        
            
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        private IReadOnlyList<ReservationWithNavigationPropertiesDto> ReservationList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateReservation { get; set; }
        private bool CanEditReservation { get; set; }
        private bool CanDeleteReservation { get; set; }
        private ReservationCreateDto NewReservation { get; set; }
        private Validations NewReservationValidations { get; set; } = new();
        private ReservationUpdateDto EditingReservation { get; set; }
        private Validations EditingReservationValidations { get; set; } = new();
        private Guid EditingReservationId { get; set; }
        private Modal CreateReservationModal { get; set; } = new();
        private Modal EditReservationModal { get; set; } = new();
        private GetReservationsInput Filter { get; set; }
        private DataGridEntityActionsColumn<ReservationWithNavigationPropertiesDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "reservation-create-tab";
        protected string SelectedEditTab = "reservation-edit-tab";
        private ReservationWithNavigationPropertiesDto? SelectedReservation;
        private IReadOnlyList<LookupDto<Guid>> UserProfilesCollection { get; set; } = new List<LookupDto<Guid>>();
private IReadOnlyList<LookupDto<Guid>> SitePropertiesCollection { get; set; } = new List<LookupDto<Guid>>();

        
        
        
        
        private List<ReservationWithNavigationPropertiesDto> SelectedReservations { get; set; } = new();
        private bool AllReservationsSelected { get; set; }
        
        public Reservations()
        {
            NewReservation = new ReservationCreateDto();
            EditingReservation = new ReservationUpdateDto();
            Filter = new GetReservationsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            ReservationList = new List<ReservationWithNavigationPropertiesDto>();
            
            
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetUserProfileCollectionLookupAsync();


            await GetSitePropertyCollectionLookupAsync();


            
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
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Reservations"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewReservation"], async () =>
            {
                await OpenCreateReservationModalAsync();
            }, IconName.Add, requiredPolicyName: AhlanFeekumPermissions.Reservations.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateReservation = await AuthorizationService
                .IsGrantedAsync(AhlanFeekumPermissions.Reservations.Create);
            CanEditReservation = await AuthorizationService
                            .IsGrantedAsync(AhlanFeekumPermissions.Reservations.Edit);
            CanDeleteReservation = await AuthorizationService
                            .IsGrantedAsync(AhlanFeekumPermissions.Reservations.Delete);
                            
                            
        }

        private async Task GetReservationsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await ReservationsAppService.GetListAsync(Filter);
            ReservationList = result.Items;
            TotalCount = (int)result.TotalCount;
            
            await ClearSelection();
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetReservationsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await ReservationsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("AhlanFeekum") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/reservations/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&FromeDateMin={Filter.FromeDateMin}&FromeDateMax={Filter.FromeDateMax}&ToDateMin={Filter.ToDateMin}&ToDateMax={Filter.ToDateMax}&CheckInDateMin={Filter.CheckInDateMin?.ToString("O")}&CheckInDateMax={Filter.CheckInDateMax?.ToString("O")}&CheckOutDateMin={Filter.CheckOutDateMin?.ToString("O")}&CheckOutDateMax={Filter.CheckOutDateMax?.ToString("O")}&NumberOfGuestMin={Filter.NumberOfGuestMin}&NumberOfGuestMax={Filter.NumberOfGuestMax}&PriceMin={Filter.PriceMin}&PriceMax={Filter.PriceMax}&DiscountMin={Filter.DiscountMin}&DiscountMax={Filter.DiscountMax}&ReservationStatus={Filter.ReservationStatus}&Notes={HttpUtility.UrlEncode(Filter.Notes)}&ReservationPaymentMethod={Filter.ReservationPaymentMethod}&IsPaid={Filter.IsPaid}&Description={HttpUtility.UrlEncode(Filter.Description)}&UserProfileId={Filter.UserProfileId}&SitePropertyId={Filter.SitePropertyId}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<ReservationWithNavigationPropertiesDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetReservationsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateReservationModalAsync()
        {
            NewReservation = new ReservationCreateDto{
                CheckInDate = DateTime.Now,
CheckOutDate = DateTime.Now,
FromeDate = DateOnly.FromDateTime(DateTime.Now),
ToDate = DateOnly.FromDateTime(DateTime.Now),

                UserProfileId = UserProfilesCollection.Select(i=>i.Id).FirstOrDefault(),
SitePropertyId = SitePropertiesCollection.Select(i=>i.Id).FirstOrDefault(),

            };

            SelectedCreateTab = "reservation-create-tab";
            
            
            await NewReservationValidations.ClearAll();
            await CreateReservationModal.Show();
        }

        private async Task CloseCreateReservationModalAsync()
        {
            NewReservation = new ReservationCreateDto{
                CheckInDate = DateTime.Now,
CheckOutDate = DateTime.Now,
FromeDate = DateOnly.FromDateTime(DateTime.Now),
ToDate = DateOnly.FromDateTime(DateTime.Now),

                UserProfileId = UserProfilesCollection.Select(i=>i.Id).FirstOrDefault(),
SitePropertyId = SitePropertiesCollection.Select(i=>i.Id).FirstOrDefault(),

            };
            await CreateReservationModal.Hide();
        }

        private async Task OpenEditReservationModalAsync(ReservationWithNavigationPropertiesDto input)
        {
            SelectedEditTab = "reservation-edit-tab";
            
            
            var reservation = await ReservationsAppService.GetWithNavigationPropertiesAsync(input.Reservation.Id);
            
            EditingReservationId = reservation.Reservation.Id;
            EditingReservation = ObjectMapper.Map<ReservationDto, ReservationUpdateDto>(reservation.Reservation);
            
            await EditingReservationValidations.ClearAll();
            await EditReservationModal.Show();
        }

        private async Task DeleteReservationAsync(ReservationWithNavigationPropertiesDto input)
        {
            await ReservationsAppService.DeleteAsync(input.Reservation.Id);
            await GetReservationsAsync();
        }

        private async Task CreateReservationAsync()
        {
            try
            {
                if (await NewReservationValidations.ValidateAll() == false)
                {
                    return;
                }

                await ReservationsAppService.CreateAsync(NewReservation);
                await GetReservationsAsync();
                await CloseCreateReservationModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditReservationModalAsync()
        {
            await EditReservationModal.Hide();
        }

        private async Task UpdateReservationAsync()
        {
            try
            {
                if (await EditingReservationValidations.ValidateAll() == false)
                {
                    return;
                }

                // Get the current reservation to check if status changed
                var currentReservation = await ReservationsAppService.GetAsync(EditingReservationId);
                var oldStatus = currentReservation.ReservationStatus;
                var newStatus = EditingReservation.ReservationStatus;

                // Show confirmation dialog if status changed
                if (oldStatus != newStatus)
                {
                    string confirmMessage = "";
                    
                    if (newStatus == ReservationStatus.Approved)
                    {
                        confirmMessage = L["ConfirmApproveReservation", 
                            "Are you sure you want to approve this reservation? The payment will be captured and the customer will be charged."];
                    }
                    else if (newStatus == ReservationStatus.Rejected)
                    {
                        confirmMessage = L["ConfirmRejectReservation", 
                            "Are you sure you want to reject this reservation? Any held payment will be canceled and the customer will not be charged."];
                    }
                    else if (newStatus == ReservationStatus.Canceled)
                    {
                        confirmMessage = L["ConfirmCancelReservation", 
                            "Are you sure you want to cancel this reservation? Any held payment will be canceled."];
                    }
                    else
                    {
                        confirmMessage = L["ConfirmUpdateReservation", 
                            "Are you sure you want to update the reservation status? This may affect any held payments."];
                    }

                    if (!await UiMessageService.Confirm(confirmMessage))
                    {
                        return;
                    }
                }

                await ReservationsAppService.UpdateAsync(EditingReservationId, EditingReservation);
                await GetReservationsAsync();
                await EditReservationModal.Hide();                
                
                // Show success message with payment info
                if (oldStatus != newStatus)
                {
                    if (newStatus == ReservationStatus.Approved)
                    {
                        await UiMessageService.Success(L["ReservationApprovedAndPaymentCaptured", 
                            "Reservation approved successfully! Payment has been captured."]);
                    }
                    else if (newStatus == ReservationStatus.Rejected || newStatus == ReservationStatus.Canceled)
                    {
                        await UiMessageService.Success(L["ReservationUpdatedAndPaymentCanceled", 
                            "Reservation updated successfully! Any held payment has been canceled."]);
            }
                }
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









        protected virtual async Task OnFromeDateMinChangedAsync(DateOnly? fromeDateMin)
        {
            Filter.FromeDateMin = fromeDateMin;
            await SearchAsync();
        }
        protected virtual async Task OnFromeDateMaxChangedAsync(DateOnly? fromeDateMax)
        {
            Filter.FromeDateMax = fromeDateMax;
            await SearchAsync();
        }
        protected virtual async Task OnToDateMinChangedAsync(DateOnly? toDateMin)
        {
            Filter.ToDateMin = toDateMin;
            await SearchAsync();
        }
        protected virtual async Task OnToDateMaxChangedAsync(DateOnly? toDateMax)
        {
            Filter.ToDateMax = toDateMax;
            await SearchAsync();
        }
        protected virtual async Task OnCheckInDateMinChangedAsync(DateTime? checkInDateMin)
        {
            Filter.CheckInDateMin = checkInDateMin.HasValue ? checkInDateMin.Value.Date : checkInDateMin;
            await SearchAsync();
        }
        protected virtual async Task OnCheckInDateMaxChangedAsync(DateTime? checkInDateMax)
        {
            Filter.CheckInDateMax = checkInDateMax.HasValue ? checkInDateMax.Value.Date.AddDays(1).AddSeconds(-1) : checkInDateMax;
            await SearchAsync();
        }
        protected virtual async Task OnCheckOutDateMinChangedAsync(DateTime? checkOutDateMin)
        {
            Filter.CheckOutDateMin = checkOutDateMin.HasValue ? checkOutDateMin.Value.Date : checkOutDateMin;
            await SearchAsync();
        }
        protected virtual async Task OnCheckOutDateMaxChangedAsync(DateTime? checkOutDateMax)
        {
            Filter.CheckOutDateMax = checkOutDateMax.HasValue ? checkOutDateMax.Value.Date.AddDays(1).AddSeconds(-1) : checkOutDateMax;
            await SearchAsync();
        }
        protected virtual async Task OnNumberOfGuestMinChangedAsync(int? numberOfGuestMin)
        {
            Filter.NumberOfGuestMin = numberOfGuestMin;
            await SearchAsync();
        }
        protected virtual async Task OnNumberOfGuestMaxChangedAsync(int? numberOfGuestMax)
        {
            Filter.NumberOfGuestMax = numberOfGuestMax;
            await SearchAsync();
        }
        protected virtual async Task OnPriceMinChangedAsync(double? priceMin)
        {
            Filter.PriceMin = priceMin;
            await SearchAsync();
        }
        protected virtual async Task OnPriceMaxChangedAsync(double? priceMax)
        {
            Filter.PriceMax = priceMax;
            await SearchAsync();
        }
        protected virtual async Task OnDiscountMinChangedAsync(double? discountMin)
        {
            Filter.DiscountMin = discountMin;
            await SearchAsync();
        }
        protected virtual async Task OnDiscountMaxChangedAsync(double? discountMax)
        {
            Filter.DiscountMax = discountMax;
            await SearchAsync();
        }
        protected virtual async Task OnReservationStatusChangedAsync(ReservationStatus? reservationStatus)
        {
            Filter.ReservationStatus = reservationStatus;
            await SearchAsync();
        }
        protected virtual async Task OnNotesChangedAsync(string? notes)
        {
            Filter.Notes = notes;
            await SearchAsync();
        }
        protected virtual async Task OnReservationPaymentMethodChangedAsync(ReservationPaymentMethod? reservationPaymentMethod)
        {
            Filter.ReservationPaymentMethod = reservationPaymentMethod;
            await SearchAsync();
        }
        protected virtual async Task OnIsPaidChangedAsync(bool? isPaid)
        {
            Filter.IsPaid = isPaid;
            await SearchAsync();
        }
        protected virtual async Task OnDescriptionChangedAsync(string? description)
        {
            Filter.Description = description;
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
        

        private async Task GetUserProfileCollectionLookupAsync(string? newValue = null)
        {
            UserProfilesCollection = (await ReservationsAppService.GetUserProfileLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }

        private async Task GetSitePropertyCollectionLookupAsync(string? newValue = null)
        {
            SitePropertiesCollection = (await ReservationsAppService.GetSitePropertyLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }





        private Task SelectAllItems()
        {
            AllReservationsSelected = true;
            
            return Task.CompletedTask;
        }

        private Task ClearSelection()
        {
            AllReservationsSelected = false;
            SelectedReservations.Clear();
            
            return Task.CompletedTask;
        }

        private Task SelectedReservationRowsChanged()
        {
            if (SelectedReservations.Count != PageSize)
            {
                AllReservationsSelected = false;
            }
            
            return Task.CompletedTask;
        }

        private async Task DeleteSelectedReservationsAsync()
        {
            var message = AllReservationsSelected ? L["DeleteAllRecords"].Value : L["DeleteSelectedRecords", SelectedReservations.Count].Value;
            
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            if (AllReservationsSelected)
            {
                await ReservationsAppService.DeleteAllAsync(Filter);
            }
            else
            {
                await ReservationsAppService.DeleteByIdsAsync(SelectedReservations.Select(x => x.Reservation.Id).ToList());
            }

            SelectedReservations.Clear();
            AllReservationsSelected = false;

            await GetReservationsAsync();
        }


    }
}
