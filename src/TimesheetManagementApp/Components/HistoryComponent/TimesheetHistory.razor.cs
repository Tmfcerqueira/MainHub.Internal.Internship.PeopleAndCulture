using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Radzen;
using TimesheetManagement.Api.Proxy.Client.Client;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.Components.HistoryComponent
{
    public partial class TimesheetHistory
    {
        [Inject]
        ITimesheetAppRepository TimesheetRepository { get; set; } = null!;

        [Inject]
        DialogService DialogService { get; set; } = null!;

        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Parameter]
        public TimesheetModel TimesheetData { get; set; } = null!;

        private int PageSize { get; set; } = 5;
        private int Count { get; set; } = 0;

        bool _tableIsLoading = false;

        Guid PersonId { get; set; }

        private (List<TimesheetHistoryModel> timesheetHistory, int errorCode, int count) _timesheetHistory = (new List<TimesheetHistoryModel>(), 200, 0);

        protected override async Task OnInitializedAsync()
        {
            try
            {
                PersonId = await GetPersonIdAsync();

                LoadDataArgs args = new LoadDataArgs()
                {
                    Filter = "",
                    Skip = 0,
                    OrderBy = "",
                    Top = PageSize,
                };

                //history list

                await LoadHistoryData(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        async Task LoadHistoryData(LoadDataArgs args)
        {
            try
            {
                _tableIsLoading = true;

                var skip = args.Skip ?? 0;
                var page = (int)(skip / PageSize) + 1;
                var filter = args.Filter;

                _timesheetHistory = await TimesheetRepository.GetTimesheetHistory(TimesheetData.TimesheetGUID, PersonId.ToString(), page, PageSize);

                Count = _timesheetHistory.count;

                _tableIsLoading = false;
            }
            catch (ApiException ex)
            {
                await HandleError(ex.ErrorCode);
            }
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
                    _timesheetHistory.timesheetHistory = new List<TimesheetHistoryModel>();
                    await DialogService.Alert(_localizer["NoTimesheetData"], _localizer["NoDataFound"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 500:
                    await DialogService.Alert(_localizer["PossiblyNoDrafts"], _localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
            }
        }


        async Task<Guid> GetPersonIdAsync()
        {
            var response = await GraphServiceClient.Me.Request().GetAsync();
            return Guid.Parse(response.Id);
        }
    }
}
