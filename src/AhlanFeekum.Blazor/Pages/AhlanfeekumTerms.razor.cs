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
using AhlanFeekum.AhlanfeekumTerms;
using AhlanFeekum.Permissions;
using AhlanFeekum.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;



namespace AhlanFeekum.Blazor.Pages
{
    public partial class AhlanfeekumTerms
    {
        [Inject]
        protected IJSRuntime JsRuntime { get; set; }
            
        private IJSObjectReference? _jsObjectRef;
            
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        private IReadOnlyList<AhlanfeekumTermDto> AhlanfeekumTermList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateAhlanfeekumTerm { get; set; }
        private bool CanEditAhlanfeekumTerm { get; set; }
        private bool CanDeleteAhlanfeekumTerm { get; set; }
        private AhlanfeekumTermCreateDto NewAhlanfeekumTerm { get; set; }
        private Validations NewAhlanfeekumTermValidations { get; set; } = new();
        private AhlanfeekumTermUpdateDto EditingAhlanfeekumTerm { get; set; }
        private Validations EditingAhlanfeekumTermValidations { get; set; } = new();
        private Guid EditingAhlanfeekumTermId { get; set; }
        private Modal CreateAhlanfeekumTermModal { get; set; } = new();
        private Modal EditAhlanfeekumTermModal { get; set; } = new();
        private GetAhlanfeekumTermsInput Filter { get; set; }
        private DataGridEntityActionsColumn<AhlanfeekumTermDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "ahlanfeekumTerm-create-tab";
        protected string SelectedEditTab = "ahlanfeekumTerm-edit-tab";
        private AhlanfeekumTermDto? SelectedAhlanfeekumTerm;
        
        
        
        
        
        private List<AhlanfeekumTermDto> SelectedAhlanfeekumTerms { get; set; } = new();
        private bool AllAhlanfeekumTermsSelected { get; set; }
        
        public AhlanfeekumTerms()
        {
            NewAhlanfeekumTerm = new AhlanfeekumTermCreateDto();
            EditingAhlanfeekumTerm = new AhlanfeekumTermUpdateDto();
            Filter = new GetAhlanfeekumTermsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            AhlanfeekumTermList = new List<AhlanfeekumTermDto>();
            
            
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsObjectRef = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "/Pages/AhlanfeekumTerms.razor.js");
                await SetBreadcrumbItemsAsync();
                await SetToolbarItemsAsync();
                await InvokeAsync(StateHasChanged);
            }
        }  

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["AhlanfeekumTerms"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewAhlanfeekumTerm"], async () =>
            {
                await OpenCreateAhlanfeekumTermModalAsync();
            }, IconName.Add, requiredPolicyName: AhlanFeekumPermissions.AhlanfeekumTerms.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateAhlanfeekumTerm = await AuthorizationService
                .IsGrantedAsync(AhlanFeekumPermissions.AhlanfeekumTerms.Create);
            CanEditAhlanfeekumTerm = await AuthorizationService
                            .IsGrantedAsync(AhlanFeekumPermissions.AhlanfeekumTerms.Edit);
            CanDeleteAhlanfeekumTerm = await AuthorizationService
                            .IsGrantedAsync(AhlanFeekumPermissions.AhlanfeekumTerms.Delete);
                            
                            
        }

        private async Task GetAhlanfeekumTermsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await AhlanfeekumTermsAppService.GetListAsync(Filter);
            AhlanfeekumTermList = result.Items;
            TotalCount = (int)result.TotalCount;
            
            await ClearSelection();
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetAhlanfeekumTermsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await AhlanfeekumTermsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("AhlanFeekum") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/ahlanfeekum-terms/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&TermsTitle={HttpUtility.UrlEncode(Filter.TermsTitle)}&TermsAnnotation={HttpUtility.UrlEncode(Filter.TermsAnnotation)}&TermsDescription={HttpUtility.UrlEncode(Filter.TermsDescription)}&TermsIconExtension={HttpUtility.UrlEncode(Filter.TermsIconExtension)}&WhoAreWeTitle={HttpUtility.UrlEncode(Filter.WhoAreWeTitle)}&WhoAreWeAnnotation={HttpUtility.UrlEncode(Filter.WhoAreWeAnnotation)}&WhoAreWeDescription={HttpUtility.UrlEncode(Filter.WhoAreWeDescription)}&WhoAreWeIconExtension={HttpUtility.UrlEncode(Filter.WhoAreWeIconExtension)}&IsActive={Filter.IsActive}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<AhlanfeekumTermDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetAhlanfeekumTermsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateAhlanfeekumTermModalAsync()
        {
            NewAhlanfeekumTerm = new AhlanfeekumTermCreateDto{
                
                
            };

            SelectedCreateTab = "ahlanfeekumTerm-create-tab";
            
            await _jsObjectRef!.InvokeVoidAsync("FileCleanup.clearInputFiles");
            await NewAhlanfeekumTermValidations.ClearAll();
            await CreateAhlanfeekumTermModal.Show();
        }

        private async Task CloseCreateAhlanfeekumTermModalAsync()
        {
            NewAhlanfeekumTerm = new AhlanfeekumTermCreateDto{
                
                
            };
            await CreateAhlanfeekumTermModal.Hide();
        }

        private async Task OpenEditAhlanfeekumTermModalAsync(AhlanfeekumTermDto input)
        {
            SelectedEditTab = "ahlanfeekumTerm-edit-tab";
            
            await _jsObjectRef!.InvokeVoidAsync("FileCleanup.clearInputFiles");
            var ahlanfeekumTerm = await AhlanfeekumTermsAppService.GetAsync(input.Id);
            
            EditingAhlanfeekumTermId = ahlanfeekumTerm.Id;
            EditingAhlanfeekumTerm = ObjectMapper.Map<AhlanfeekumTermDto, AhlanfeekumTermUpdateDto>(ahlanfeekumTerm);
            HasSelectedAhlanfeekumTermTermsIcon = EditingAhlanfeekumTerm.TermsIconId != null && EditingAhlanfeekumTerm.TermsIconId != Guid.Empty;
HasSelectedAhlanfeekumTermWhoAreWeIcon = EditingAhlanfeekumTerm.WhoAreWeIconId != null && EditingAhlanfeekumTerm.WhoAreWeIconId != Guid.Empty;

            await EditingAhlanfeekumTermValidations.ClearAll();
            await EditAhlanfeekumTermModal.Show();
        }

        private async Task DeleteAhlanfeekumTermAsync(AhlanfeekumTermDto input)
        {
            await AhlanfeekumTermsAppService.DeleteAsync(input.Id);
            await GetAhlanfeekumTermsAsync();
        }

        private async Task CreateAhlanfeekumTermAsync()
        {
            try
            {
                if (await NewAhlanfeekumTermValidations.ValidateAll() == false)
                {
                    return;
                }

                await AhlanfeekumTermsAppService.CreateAsync(NewAhlanfeekumTerm);
                await GetAhlanfeekumTermsAsync();
                await CloseCreateAhlanfeekumTermModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditAhlanfeekumTermModalAsync()
        {
            await EditAhlanfeekumTermModal.Hide();
        }

        private async Task UpdateAhlanfeekumTermAsync()
        {
            try
            {
                if (await EditingAhlanfeekumTermValidations.ValidateAll() == false)
                {
                    return;
                }

                await AhlanfeekumTermsAppService.UpdateAsync(EditingAhlanfeekumTermId, EditingAhlanfeekumTerm);
                await GetAhlanfeekumTermsAsync();
                await EditAhlanfeekumTermModal.Hide();                
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


        private bool IsCreateFormDisabled()
        {
            return OnNewAhlanfeekumTermTermsIconLoading ||NewAhlanfeekumTerm.TermsIconId == Guid.Empty ||OnNewAhlanfeekumTermWhoAreWeIconLoading ||NewAhlanfeekumTerm.WhoAreWeIconId == Guid.Empty ;
        }
        
        private bool IsEditFormDisabled()
        {
            return OnEditAhlanfeekumTermTermsIconLoading ||EditingAhlanfeekumTerm.TermsIconId == Guid.Empty ||OnEditAhlanfeekumTermWhoAreWeIconLoading ||EditingAhlanfeekumTerm.WhoAreWeIconId == Guid.Empty ;
        }



        private int MaxAhlanfeekumTermTermsIconFileUploadSize = 1024 * 1024 * 10; //10MB
        private bool OnNewAhlanfeekumTermTermsIconLoading = false;
        private async Task OnNewAhlanfeekumTermTermsIconChanged(InputFileChangeEventArgs e)
        {
            try
            {
                if (e.FileCount is 0 or > 1 || e.File.Size > MaxAhlanfeekumTermTermsIconFileUploadSize)
                {
                    throw new UserFriendlyException(L["UploadFailedMessage"]);
                }
    
                OnNewAhlanfeekumTermTermsIconLoading = true;
                
                var result = await UploadFileAsync(e.File!);
    
                NewAhlanfeekumTerm.TermsIconId = result.Id;
                NewAhlanfeekumTerm.TermsIconExtension = Path.GetExtension(e.File.Name);
                OnNewAhlanfeekumTermTermsIconLoading = false;            
            }
            catch(Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private int MaxAhlanfeekumTermWhoAreWeIconFileUploadSize = 1024 * 1024 * 10; //10MB
        private bool OnNewAhlanfeekumTermWhoAreWeIconLoading = false;
        private async Task OnNewAhlanfeekumTermWhoAreWeIconChanged(InputFileChangeEventArgs e)
        {
            try
            {
                if (e.FileCount is 0 or > 1 || e.File.Size > MaxAhlanfeekumTermWhoAreWeIconFileUploadSize)
                {
                    throw new UserFriendlyException(L["UploadFailedMessage"]);
                }
    
                OnNewAhlanfeekumTermWhoAreWeIconLoading = true;
                
                var result = await UploadFileAsync(e.File!);
    
                NewAhlanfeekumTerm.WhoAreWeIconId = result.Id;
                OnNewAhlanfeekumTermWhoAreWeIconLoading = false;            
            }
            catch(Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
        private bool HasSelectedAhlanfeekumTermTermsIcon = false;
        private bool OnEditAhlanfeekumTermTermsIconLoading = false;
        private async Task OnEditAhlanfeekumTermTermsIconChanged(InputFileChangeEventArgs e)
        {
            try
            {
                if (e.FileCount is 0 or > 1 || e.File.Size > MaxAhlanfeekumTermTermsIconFileUploadSize)
                {
                    throw new UserFriendlyException(L["UploadFailedMessage"]);
                }
    
                OnEditAhlanfeekumTermTermsIconLoading = true;
                
                var result = await UploadFileAsync(e.File!);
    
                EditingAhlanfeekumTerm.TermsIconId = result.Id;
                EditingAhlanfeekumTerm.TermsIconExtension = Path.GetExtension(e.File.Name);

                OnEditAhlanfeekumTermTermsIconLoading = false;            
            }
            catch(Exception ex)
            {
                await HandleErrorAsync(ex);
            }            
        }

        private bool HasSelectedAhlanfeekumTermWhoAreWeIcon = false;
        private bool OnEditAhlanfeekumTermWhoAreWeIconLoading = false;
        private async Task OnEditAhlanfeekumTermWhoAreWeIconChanged(InputFileChangeEventArgs e)
        {
            try
            {
                if (e.FileCount is 0 or > 1 || e.File.Size > MaxAhlanfeekumTermWhoAreWeIconFileUploadSize)
                {
                    throw new UserFriendlyException(L["UploadFailedMessage"]);
                }
    
                OnEditAhlanfeekumTermWhoAreWeIconLoading = true;
                
                var result = await UploadFileAsync(e.File!);
    
                EditingAhlanfeekumTerm.WhoAreWeIconId = result.Id;
                OnEditAhlanfeekumTermWhoAreWeIconLoading = false;            
            }
            catch(Exception ex)
            {
                await HandleErrorAsync(ex);
            }            
        }




        private async Task<AppFileDescriptorDto> UploadFileAsync(IBrowserFile file)
        {
            using (var ms = new MemoryStream())
            {
                await file.OpenReadStream(long.MaxValue).CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                
                return await AhlanfeekumTermsAppService.UploadFileAsync(new RemoteStreamContent(ms, file.Name, file.ContentType));
            }
        }



        private async Task DownloadFileAsync(Guid fileId)
        {
            var token = (await AhlanfeekumTermsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("AhlanFeekum") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/ahlanfeekum-terms/file?DownloadToken={token}&FileId={fileId}", forceLoad: true);
        }

        protected virtual async Task OnTermsTitleChangedAsync(string? termsTitle)
        {
            Filter.TermsTitle = termsTitle;
            await SearchAsync();
        }
        protected virtual async Task OnTermsAnnotationChangedAsync(string? termsAnnotation)
        {
            Filter.TermsAnnotation = termsAnnotation;
            await SearchAsync();
        }
        protected virtual async Task OnTermsDescriptionChangedAsync(string? termsDescription)
        {
            Filter.TermsDescription = termsDescription;
            await SearchAsync();
        }
        protected virtual async Task OnTermsIconExtensionChangedAsync(string? termsIconExtension)
        {
            Filter.TermsIconExtension = termsIconExtension;
            await SearchAsync();
        }
        protected virtual async Task OnWhoAreWeTitleChangedAsync(string? whoAreWeTitle)
        {
            Filter.WhoAreWeTitle = whoAreWeTitle;
            await SearchAsync();
        }
        protected virtual async Task OnWhoAreWeAnnotationChangedAsync(string? whoAreWeAnnotation)
        {
            Filter.WhoAreWeAnnotation = whoAreWeAnnotation;
            await SearchAsync();
        }
        protected virtual async Task OnWhoAreWeDescriptionChangedAsync(string? whoAreWeDescription)
        {
            Filter.WhoAreWeDescription = whoAreWeDescription;
            await SearchAsync();
        }
        protected virtual async Task OnWhoAreWeIconExtensionChangedAsync(string? whoAreWeIconExtension)
        {
            Filter.WhoAreWeIconExtension = whoAreWeIconExtension;
            await SearchAsync();
        }
        protected virtual async Task OnIsActiveChangedAsync(bool? isActive)
        {
            Filter.IsActive = isActive;
            await SearchAsync();
        }
        





        private Task SelectAllItems()
        {
            AllAhlanfeekumTermsSelected = true;
            
            return Task.CompletedTask;
        }

        private Task ClearSelection()
        {
            AllAhlanfeekumTermsSelected = false;
            SelectedAhlanfeekumTerms.Clear();
            
            return Task.CompletedTask;
        }

        private Task SelectedAhlanfeekumTermRowsChanged()
        {
            if (SelectedAhlanfeekumTerms.Count != PageSize)
            {
                AllAhlanfeekumTermsSelected = false;
            }
            
            return Task.CompletedTask;
        }

        private async Task DeleteSelectedAhlanfeekumTermsAsync()
        {
            var message = AllAhlanfeekumTermsSelected ? L["DeleteAllRecords"].Value : L["DeleteSelectedRecords", SelectedAhlanfeekumTerms.Count].Value;
            
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            if (AllAhlanfeekumTermsSelected)
            {
                await AhlanfeekumTermsAppService.DeleteAllAsync(Filter);
            }
            else
            {
                await AhlanfeekumTermsAppService.DeleteByIdsAsync(SelectedAhlanfeekumTerms.Select(x => x.Id).ToList());
            }

            SelectedAhlanfeekumTerms.Clear();
            AllAhlanfeekumTermsSelected = false;

            await GetAhlanfeekumTermsAsync();
        }


    }
}
