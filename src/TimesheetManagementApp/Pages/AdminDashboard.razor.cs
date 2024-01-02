using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Radzen;
using TimesheetManagement.Api.Proxy.Client.Client;
using TimesheetManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.Pages
{
    public partial class AdminDashboard
    {
        [Inject]
        ITimesheetAppRepository TimesheetRepository { get; set; } = null!;

        [Inject]
        DialogService DialogService { get; set; } = null!;

        private int Approved { get; set; } = 0;
        private int Draft { get; set; } = 0;
        private int Submitted { get; set; } = 0;
        private int Rejected { get; set; } = 0;

        bool _isLoading = false;

        private (List<TimesheetModel> timesheets, int errorCode, int count) _timesheets = (new List<TimesheetModel>(), 200, 0);


        protected override async Task OnInitializedAsync()
        {
            try
            {
                _isLoading = true;

                var result = (new List<TimesheetModel>(), 200, 0);
                int resultCount;
                var count = 0;
                var pageSize = 20;
                var page = 1;

                result = await TimesheetRepository.GetAllTimesheetsAsync(page, pageSize, ApprovalStatus.All);
                resultCount = result.Item3;
                _timesheets.timesheets.AddRange(result.Item1);
                count += result.Item1.Count;
                while (count < resultCount)
                {
                    page++;
                    result = await TimesheetRepository.GetAllTimesheetsAsync(page, pageSize, ApprovalStatus.All);
                    _timesheets.timesheets.AddRange(result.Item1);
                    count += result.Item1.Count;
                }

                _timesheets.timesheets.ForEach(t =>
                {
                    switch (t.ApprovalStatus)
                    {
                        case Common.ApprovalStatus.Approved: Approved++; break;
                        case Common.ApprovalStatus.Draft: Draft++; break;
                        case Common.ApprovalStatus.Submitted: Submitted++; break;
                        case Common.ApprovalStatus.Rejected: Rejected++; break;
                    }
                });

                _isLoading = false;
            }
            catch (ApiException ex)
            {
                await HandleError(ex.ErrorCode);
            }
            catch (Exception)
            {
                await HandleError(404);
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
                    _timesheets.timesheets = new List<TimesheetModel>();
                    await DialogService.Alert(_localizer["NoTimesheetData"], _localizer["NoDataFound"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 500:
                    await DialogService.Alert(_localizer["PossiblyNoDrafts"], _localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
            }
        }
    }
}
