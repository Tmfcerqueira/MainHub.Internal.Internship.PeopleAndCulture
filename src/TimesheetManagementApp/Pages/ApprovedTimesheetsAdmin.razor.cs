using System.Net.NetworkInformation;
using System;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Components.ActivityComponent;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Microsoft.JSInterop;
using Radzen;
using ProxyModel = TimesheetManagement.Api.Proxy.Client.Model;
using TimesheetManagement.Api.Proxy.Client.Client;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.Pages
{
    public partial class ApprovedTimesheetsAdmin
    {
        [Inject]
        ITimesheetAppRepository TimesheetRepository { get; set; } = null!;

        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Inject]
        DialogService DialogService { get; set; } = null!;

        [Inject]
        TooltipService TooltipService { get; set; } = null!;

        [Inject]
        NavigationManager NavigationManager { get; set; } = null!;


        private bool _isLoading;

        private int PageSize { get; set; } = 4;
        private int Count { get; set; } = 0;

        private (List<TimesheetModel> timesheets, int errorCode, int count) _timesheets = (new List<TimesheetModel>(), 200, 0);

        string _help = "";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _help = _localizer["ToolTip"];

                LoadDataArgs args = new LoadDataArgs()
                {
                    Filter = "",
                    Skip = 0,
                    OrderBy = "",
                    Top = PageSize,
                };

                await LoadData(args);
            }
            catch (ApiException ex)
            {
                await HandleError(ex.ErrorCode);
            }
            catch (Exception ex)
            {
                await DialogService.Alert(ex.Message, _localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
            }
        }

        async Task LoadData(LoadDataArgs args)
        {
            _isLoading = true;

            var skip = args.Skip ?? 0;
            var page = (int)(skip / PageSize) + 1;
            var filter = args.Filter;

            _timesheets = await TimesheetRepository.GetAllTimesheetsAsync(page, PageSize, ProxyModel.ApprovalStatus.Approved);

            Count = _timesheets.count;

            _isLoading = false;
        }

        async Task<Guid> GetPersonIdAsync()
        {
            var response = await GraphServiceClient.Me.Request().GetAsync();
            return Guid.Parse(response.Id);
        }

        private async Task OnRowDoubleClick(Guid timesheetGuid, Guid personGuid)
        {
            NavigationManager.NavigateTo($"/activities/{timesheetGuid}/{personGuid}");
        }


        private async Task HandleError(int errorCode)
        {
            switch (errorCode)
            {
                case 400:
                    await DialogService.Alert(_localizer["InvalidRequest"], _localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 401:
                    await DialogService.Alert(_localizer["NotAuthorizedResource"], _localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 404:
                    _timesheets.timesheets = new List<TimesheetModel>();
                    await DialogService.Alert(_localizer["NoTimesheetData"], _localizer["NoDataFound"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 500:
                    await DialogService.Alert(_localizer["PossiblyNoDrafts"], _localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
            }
        }

        void ShowHelpTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, _help, options);
    }
}
