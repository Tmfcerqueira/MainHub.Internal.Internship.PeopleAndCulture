using global::TimesheetManagement.Api.Proxy.Client.Model;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Radzen;
using System.Drawing.Printing;
using TimesheetManagement.Api.Proxy.Client.Client;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.Pages
{
    public partial class Dashboard
    {
        [Inject]
        ITimesheetAppRepository TimesheetRepository { get; set; } = null!;

        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Inject]
        NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        DialogService DialogService { get; set; } = null!;

        private int PageSize { get; set; } = 4;
        private int Count { get; set; } = 0;

        Guid PersonGuid { get; set; }

        bool _isLoading = false;

        private int Approved { get; set; } = 0;
        private int Draft { get; set; } = 0;
        private int Submitted { get; set; } = 0;
        private int Rejected { get; set; } = 0;


        private (List<TimesheetModel> timesheets, int errorCode, int count) _timesheets = (new List<TimesheetModel>(), 200, 0);


        protected override async Task OnInitializedAsync()
        {
            try
            {
                _isLoading = true;

                PersonGuid = await GetPersonIdAsync();

                List<TimesheetModel> resultAppList = new List<TimesheetModel>();
                var result = (new List<TimesheetModel>(), 200, 0);
                int resultCount;
                var count = 0;
                var pageSize = 20;
                var page = 1;

                result = await TimesheetRepository.GetAllTimesheetsFromOnePerson(PersonGuid, 0, 0, ApprovalStatus.All, page, pageSize);
                resultCount = result.Item3;
                resultAppList.AddRange(result.Item1);
                count += result.Item1.Count;
                while (count < resultCount)
                {
                    page++;
                    result = await TimesheetRepository.GetAllTimesheetsFromOnePerson(PersonGuid, 0, 0, ApprovalStatus.All, page, pageSize);
                    resultAppList.AddRange(result.Item1);
                    count += result.Item1.Count;
                }

                resultAppList.ForEach(t =>
                {
                    switch (t.ApprovalStatus)
                    {
                        case Common.ApprovalStatus.Approved: Approved++; break;
                        case Common.ApprovalStatus.Draft: Draft++; break;
                        case Common.ApprovalStatus.Submitted: Submitted++; break;
                        case Common.ApprovalStatus.Rejected: Rejected++; break;
                    }
                });

                _timesheets.timesheets = resultAppList;

                _isLoading = false;
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


        async Task<Guid> GetPersonIdAsync()
        {
            var response = await GraphServiceClient.Me.Request().GetAsync();
            return Guid.Parse(response.Id);
        }

        private void RedirectToTimesheetsList()
        {
            NavigationManager.NavigateTo("/timesheets");
        }
    }
}
