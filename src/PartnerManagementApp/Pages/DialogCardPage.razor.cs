using MainHub.Internal.PeopleAndCulture.PartnerManagement.Components;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PartnerManagement.App.Models;
using PartnerManagement.App.Repository;
using Radzen;
using Radzen.Blazor;
using PartnerManagement.Api.Proxy.Client.Client;
using System.Text.RegularExpressions;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.Pages
{
    [Authorize(Policy = "Supervisor")]
    public partial class DialogCardPage
    {
        //Injects

        [Inject]
        private DialogService DialogService { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        private PartnerAppRepository PartnerRepository { get; set; } = null!;
        [Inject]
        private TooltipService TooltipService { get; set; } = null!;
        //Parameters

        [Parameter]
        public Guid PGuid { get; set; }

        [CascadingParameter]
        protected PartnerModel Partners { get; set; } = null!;

        //IEnumerables 
        private (List<ContactModel> contacts, int count) _contactModel_Data = (new List<ContactModel>(), 0);


        // Propriedades
        private PartnerModel Partner { get; set; } = new();
        private ContactModel Contact { get; set; } = null!;
        RadzenDataGrid<ContactModel> Grid { get; set; } = null!;
        private bool IsLoading { get; set; }
        private int Count { get; set; }
        private int Picker { get; set; }
        //Constructors 
        private ContactModel? _model = new();
        private string? FilterSearch { get; set; }
        private bool IsSearching { get; set; }
        private string SearchTerm { get; set; }

        private readonly PartnerApiPagination _partnerApiPagination = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                IsLoading = true;

                LoadDataArgs args = new LoadDataArgs()
                {
                    Filter = "",
                    Skip = 0,
                    OrderBy = "",
                    Top = _partnerApiPagination.PageSize,
                };
                var partnerGuidValidated = (PGuid == Guid.Empty) ? Guid.Empty : PGuid;

                Partner = await PartnerRepository.Get_Partner_By_Guid_Async(partnerGuidValidated);

                await LoadData(args);
                IsLoading = false;
            }
            catch (ApiException iex)
            {
                ApiHandlerInitialized(iex.ErrorCode);
            }
        }
        private async Task LoadData(LoadDataArgs args)
        {
            try
            {
                var partnerGuidValidated = (PGuid == Guid.Empty) ? Guid.Empty : PGuid;

                var skip = args.Skip ?? 0;

                var page = (int)(skip / _partnerApiPagination.PageSize) + 1;

                FilterSearch = "";

                _partnerApiPagination.Start_Page_Number = page;

                _contactModel_Data = await PartnerRepository.Get_All_Contacts_Async(partnerGuidValidated, _partnerApiPagination.Start_Page_Number, _partnerApiPagination.PageSize, FilterSearch);

                Count = _contactModel_Data.count;

            }
            catch (ApiException iex)
            {
                ApiHandler(iex.ErrorCode);
            }
        }
        private async Task OnChangeSearch()
        {
            IsSearching = true;
            IsLoading = true;
            _contactModel_Data = await PartnerRepository.Get_All_Contacts_Async(PGuid, 1, _partnerApiPagination.PageSize, FilterSearch);

            Count = _contactModel_Data.count;
            IsLoading = false;
        }
        private async Task ClearSearch()
        {

            SearchTerm = "";
            FilterSearch = SearchTerm;
            await OnChangeSearch();

            IsSearching = false;
        }

        void ShowTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, $"{Localization["TtpContactList"]}", options);

        private async Task OnDoubleClick(DataGridRowMouseEventArgs<ContactModel> args)
        {
            NavigationManager.NavigateTo($"ViewContactInfo/{args.Data.PartnerGUID}/{args.Data.ContactGUID}");
        }
        private async Task RedirectContactRequest()
        {
            try
            {
                var model = await PartnerRepository.Get_Partner_By_Guid_Async(PGuid);
                if (model != null)
                {
                    await DialogService.OpenAsync<DialogRequestContactComponent>($"{Localization["ContactCreation"]}", new Dictionary<string, object>() { { "Partner", model } }, new DialogOptions()
                    {
                        Width = "1260px",
                        Height = "416px",
                        ShowTitle = true,
                        CloseDialogOnOverlayClick = true,
                        Style = "margin-top:0.5%;overflow:hidden !important;",
                    });
                    StateHasChanged();
                }
            }
            catch (Exception)
            {
                DialogService.Alert($"{Localization["TryAgain"]}", $"{Localization["ActionFailed"]}", new AlertOptions() { OkButtonText = "Ok" });
            }
        }
        private void RedirectToPartners()
        {
            NavigationManager.NavigateTo($"ShowPartnerData/");
        }
        private void RedirectToContacts()
        {
            NavigationManager.NavigateTo($"DialogCardPage/{PGuid}/");
        }
        private void RedirectToPartnerMenu()
        {
            NavigationManager.NavigateTo($"ViewPartnerInfo/{PGuid}");
        }
        private async Task ApiHandler(int errorCode)
        {
            switch (errorCode)
            {
                case 401:
                    await DialogService.Alert(Localization["NotAuthorized"], Localization["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 404:
                    _contactModel_Data.contacts = new List<ContactModel>();
                    await DialogService.Alert(Localization["NoDataFound"], Localization["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
            }
        }
        private async Task ApiHandlerInitialized(int errorCode)
        {
            switch (errorCode)
            {
                case 401:
                    await DialogService.Alert(Localization["NotAuthorized"], Localization["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 404:
                    Partner = new PartnerModel();
                    await DialogService.Alert(Localization["NoDataFound"], Localization["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
            }
        }
    }
}

