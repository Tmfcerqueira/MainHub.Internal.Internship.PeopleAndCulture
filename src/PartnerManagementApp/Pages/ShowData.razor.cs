using System;
using System.Drawing.Printing;
using System.Reflection;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PartnerManagement.Api.Proxy.Client.Client;
using PartnerManagement.App.Models;
using PartnerManagement.App.Repository;
using Radzen;
using Radzen.Blazor;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.Pages
{
    [Authorize(Policy = "Supervisor")]
    public partial class ShowData
    {
        [Inject]
        private PartnerAppRepository PartnerRepository { get; set; } = null!;

        [Inject]
        private DialogService DialogService { get; set; } = null!;
        [Inject]
        private TooltipService TooltipService { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        private readonly PartnerApiPagination _partnerApiPagination = new();

        private RadzenDataGrid<PartnerModel> Grid { get; set; } = null!;

        private (List<PartnerModel> partners, int count) _partnerModel_Data = (new List<PartnerModel>(), 0);

        PartnerModel _model = new PartnerModel();
        private bool IsLoading { get; set; } = false;
        private int Count { get; set; }
        private int Picker { get; set; }
        private string? FilterSearch { get; set; }
        private bool IsSearching { get; set; } = false;
        private string SearchTerm { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            IsSearching = false;
            IsLoading = true;

            LoadDataArgs args = new LoadDataArgs()
            {
                Filter = "",
                Skip = 0,
                OrderBy = "",
                Top = _partnerApiPagination.PageSize,
            };

            await LoadData(args);
        }
        private async Task LoadData(LoadDataArgs args)
        {
            try
            {
                var skip = args.Skip ?? 0;

                var page = (int)(skip / _partnerApiPagination.PageSize) + 1;

                var filter = args.Filter;

                _partnerApiPagination.Start_Page_Number = page;

                FilterSearch = "";

                _partnerModel_Data = await PartnerRepository.Get_All_Partners_Async(_partnerApiPagination.Start_Page_Number, _partnerApiPagination.PageSize, FilterSearch);

                Count = _partnerModel_Data.count;
                IsLoading = false;
            }
            catch (ApiException apiex)
            {
                ApiHandler(apiex.ErrorCode);
                IsLoading = false;
            }
        }
        private async Task Search()
        {
            IsSearching = true;

            SearchTerm = FilterSearch;
            _partnerModel_Data = await PartnerRepository.Get_All_Partners_Async(1, _partnerApiPagination.PageSize, SearchTerm);

            Count = _partnerModel_Data.count;
            StateHasChanged();
        }
        void ShowTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, $"{Localization["ToolTipPartnerList"]}", options);
        void ShowTooltip2(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, $"{Localization["Search"]}", options);
        void ShowTooltip3(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, $"{Localization["CancelSearch"]}", options);
        void ShowTooltip4(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, $"{Localization["CreatePartner"]}", options);

        private async Task ClearSearch()
        {
            IsLoading = true;
            SearchTerm = "";
            FilterSearch = SearchTerm;

            _partnerApiPagination.Start_Page_Number = _partnerModel_Data.count;

            Count = _partnerModel_Data.count;
            await Search();

            IsSearching = false;
            IsLoading = false;
            StateHasChanged();
        }
        private async Task RedirectToCreatePartners()
        {
            await DialogService.OpenAsync<DialogPartnerRequestComponent>($"{Localization["PartnerCreation"]}", null, new DialogOptions()
            {
                Width = "1260px",
                ShowTitle = true,
                CloseDialogOnOverlayClick = true,
                Style = "margin-top:4%;"
            });
        }
        private void RedirectContactRequest()
        {
            NavigationManager.NavigateTo($"ContactRequestDpdwn/");
        }
        private void RedirectToPartners()
        {
            NavigationManager.NavigateTo($"ShowPartnerData/");
        }
        private async Task RedirectToDialogCardPage(Guid partnerGuid)
        {
            DialogService.Close();
            _model.PartnerGUID = partnerGuid;
            NavigationManager.NavigateTo($"ViewPartnerInfo/{_model.PartnerGUID}");

        }

        private async Task ApiHandler(int errorCode)
        {
            switch (errorCode)
            {
                case 401:
                    await DialogService.Alert(Localization["NotAuthorized"], Localization["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 404:
                    _partnerModel_Data.partners = new List<PartnerModel>();
                    await DialogService.Alert(Localization["NoDataFound"], Localization["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
            }
        }
    }
}
