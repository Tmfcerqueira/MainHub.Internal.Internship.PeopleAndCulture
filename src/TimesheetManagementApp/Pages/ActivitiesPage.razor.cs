using System;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using MainHub.Internal.PeopleAndCulture.Common;
using MainHub.Internal.PeopleAndCulture.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.JSInterop;
using Polly;
using Radzen;
using TimesheetManagement.Api.Proxy.Client.Client;
using ProxyModel = TimesheetManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.Pages
{
    public partial class ActivitiesPage
    {
        [Inject]
        ITimesheetAppRepository TimesheetRepository { get; set; } = null!;

        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Inject]
        DialogService DialogService { get; set; } = null!;

        private TimesheetModel Timesheet { get; set; } = null!;

        [Parameter]
        public Guid TimesheetGuid { get; set; }
        [Parameter]
        public Guid PersonGuid { get; set; }

        private (List<TimesheetModel> timesheets, int errorCode, int count) _timesheets = (new List<TimesheetModel>(), 200, 0);

        private bool _isLoading;
        private int PageSize { get; set; } = 6;
        private int Count { get; set; } = 0;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                LoadDataArgs args = new LoadDataArgs()
                {
                    Filter = "",
                    Skip = 0,
                    OrderBy = "",
                    Top = PageSize,
                };

                var timesheet = _timesheets.timesheets.FirstOrDefault(a => a.TimesheetGUID == TimesheetGuid);

                if (timesheet != null)
                {
                    PersonGuid = timesheet.PersonGUID;
                }

                Timesheet = await TimesheetRepository.GetOneTimesheetFromOnePerson(PersonGuid, TimesheetGuid);

                Timesheet.PersonGUID = PersonGuid;
                StateHasChanged();
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
            try
            {
                _isLoading = true;

                var skip = args.Skip ?? 0;
                var page = (int)(skip / PageSize) + 1;

                _timesheets = await TimesheetRepository.GetAllTimesheetsAsync(page, PageSize, ProxyModel.ApprovalStatus.Submitted);

                Count = _timesheets.count;

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

        async Task<Guid> GetPersonIdAsync()
        {
            var response = await GraphServiceClient.Me.Request().GetAsync();
            return Guid.Parse(response.Id);
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
