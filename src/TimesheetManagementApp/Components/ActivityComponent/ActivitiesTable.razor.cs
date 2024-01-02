using System;
using System.Collections.Generic;
using System.Diagnostics;
using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Graph;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor.Rendering;
using Proxy = TimesheetManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Components.HistoryComponent;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;
using TimesheetManagement.Api.Proxy.Client.Client;
using System.Reflection.Metadata;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.Components.ActivityComponent
{
    public partial class ActivitiesTable
    {
        [Parameter]
        public Guid TimesheetId { get; set; }

        [Parameter]
        public Guid PersonId { get; set; }


        [Inject]
        ITimesheetAppRepository TimesheetRepository { get; set; } = null!;

        //for person guid
        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Inject]
        DialogService DialogService { get; set; } = null!;

        [Inject]
        IJSRuntime JSRuntime { get; set; } = null!;

        [Parameter]
        public TimesheetModel TimesheetModel { get; set; } = null!;

        private (List<TimesheetActivityRecordsModel> activityRecords, int errorCode, int count) _activityRecords = (new List<TimesheetActivityRecordsModel>(), 200, 0);

        List<ProjectModel> _projects = new List<ProjectModel>();

        List<ProjectActivityModel> _projectActivities = new List<ProjectActivityModel>();

        private (List<TimesheetModel> timesheets, int errorCode, int count) _timesheets = (new List<TimesheetModel>(), 200, 0);

        public ApprovalStatus _timesheetStatus;

        readonly TypeOfWork[] _workTypes = (TypeOfWork[])Enum.GetValues(typeof(TypeOfWork));

        public readonly TimesheetActivityRecordsModel ActivityRecordsModel = new TimesheetActivityRecordsModel();

        public readonly TimesheetActivityModel ActivityModel = new TimesheetActivityModel();

        readonly int _currentYear = DateTime.Now.Year;
        readonly int _currentMonth = DateTime.Now.Month;

        readonly string[] _headers = new string[] { "Project", "Activity", "Type", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };

        private TimesheetModel? _matchingTimesheet;

        Guid PersonGuid { get; set; }

        bool _isSubmitted;

        private bool IsLoading { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            PersonGuid = await GetPersonIdAsync();

            Guid projectGuid = Guid.Empty;
            Guid timesheetGuid = Guid.Empty;

            _matchingTimesheet = await TimesheetRepository.GetOneTimesheetFromOnePerson(PersonId, TimesheetId);

            if (_matchingTimesheet != null)
            {
                TimesheetModel = _matchingTimesheet;

                timesheetGuid = _matchingTimesheet.TimesheetGUID;
                _timesheetStatus = _matchingTimesheet.ApprovalStatus;

                if (_matchingTimesheet.ApprovalStatus == ApprovalStatus.Approved || _matchingTimesheet.ApprovalStatus == ApprovalStatus.Submitted)
                {
                    _isSubmitted = true;
                }
            }

            //Get projects
            _projects = await TimesheetRepository.GetProjectsForUser(PersonGuid);

            //Get activityProjects
            _projectActivities = await TimesheetRepository.GetProjectActivities(PersonGuid, projectGuid);

            //Get activityRecords
            try
            {
                _activityRecords = await TimesheetRepository.GetTimesheetActivityRecords(PersonGuid, timesheetGuid);
            }
            catch (ApiException ex)
            {
                await HandleError(ex.ErrorCode);
            }

            _enabledRows = new bool[_activityRecords.activityRecords.Count()];

            IsLoading = false;
        }
        bool _isClosed = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _isClosed = false;
            }

            var sidebarToggleValue = await JSRuntime.InvokeAsync<bool>("eval", "document.body.classList.contains('sb-sidenav-toggled')");

            bool isSidebarClosed = sidebarToggleValue;

            if (!isSidebarClosed && !_isClosed)
            {
                await JSRuntime.InvokeVoidAsync("toggleSidenavbar");
                _isClosed = true;
            }
        }

        async Task OnCreate(TimesheetActivityRecordsModel tarm)
        {
            Guid timesheetGuid = Guid.Empty;

            int totalHours = 0;
            List<int> dayHoursList = new List<int>();

            _matchingTimesheet = await TimesheetRepository.GetOneTimesheetFromOnePerson(PersonGuid, TimesheetId);

            if (_matchingTimesheet != null)
            {
                timesheetGuid = _matchingTimesheet.TimesheetGUID;
            }

            foreach (var day in tarm.Days)
            {
                totalHours += day.Hours;
                dayHoursList.Add(day.Hours);
            }

            tarm.TimesheetGUID = timesheetGuid;

            if (tarm.ProjectGUID == Guid.Empty || tarm.ActivityGUID == Guid.Empty || tarm.TypeOfWork == 0 || totalHours <= 0)
            {
                await DialogService.Alert(_localizer["SelectActivities"], _localizer["CreateActivities"], new AlertOptions() { OkButtonText = "OK" });
            }
            else if (dayHoursList.Any(h => h < 0 || h > 24))
            {
                await DialogService.Alert(_localizer["DayHours"], _localizer["CreateActivities"], new AlertOptions() { OkButtonText = "OK" });
            }
            else
            {

                await TimesheetRepository.CreateActivities(PersonGuid, tarm.TimesheetActivityGUID, PersonGuid, tarm);
                await DialogService.Alert(_localizer["ActivitiesAdded"], _localizer["CreateActivities"], new AlertOptions() { OkButtonText = "OK" });
                _showForm = false;
                ActivityRecordsModel.ProjectGUID = default;
                ActivityRecordsModel.ActivityGUID = default;
                ActivityRecordsModel.TypeOfWork = default;

                for (int i = 0; i < ActivityRecordsModel.Days.Length; i++)
                {
                    ActivityRecordsModel.Days[i].Hours = default;
                }

                //Reload
                await OnInitializedAsync();
            }
        }

        bool[] _enabledRows;

        private void OnEdit(int idx)
        {
            _enabledRows[idx] = true;
        }

        async Task OnUpdate(TimesheetActivityRecordsModel tarm, int idx)
        {
            _enabledRows[idx] = false;

            Guid timesheetGuid = Guid.Empty;

            if (_matchingTimesheet != null)
            {
                timesheetGuid = _matchingTimesheet.TimesheetGUID;
            }

            tarm.TimesheetGUID = timesheetGuid;

            await TimesheetRepository.CreateActivities(PersonGuid, tarm.TimesheetActivityGUID, PersonGuid, tarm);
            await DialogService.Alert(_localizer["ActivitiesUpdated"], _localizer["UpdateActivities"], new AlertOptions() { OkButtonText = "OK" });
            //Reload the current page
            await OnInitializedAsync();
        }

        async Task OnSubmitTimesheet()
        {
            bool confirmed = await DialogService.Confirm(_localizer["SubmitTheTimesheet"], _localizer["Submit"], new ConfirmOptions() { OkButtonText = _localizer["Yes"], CancelButtonText = _localizer["No"] }) ?? false;

            if (confirmed)
            {
                TimesheetModel.ApprovalStatus = ApprovalStatus.Submitted;
                TimesheetModel.DateOfSubmission = DateTime.Now;

                var result = await TimesheetRepository.UpdateTimesheetAsync(TimesheetModel.PersonGUID, TimesheetModel.TimesheetGUID, TimesheetModel, PersonGuid);

                if (result)
                {
                    //Close current form
                    DialogService.Close();

                    _showForm = false;

                    await DialogService.Alert(_localizer["TimesheetSubmitted"], _localizer["SubmitTimesheet"], new AlertOptions() { OkButtonText = "Ok" });

                    await OnInitializedAsync();
                }
                else
                {
                    //Error
                    await DialogService.Alert(_localizer["ErrorSubmittingTimesheet"], _localizer["SubmitTimesheet"], new AlertOptions() { OkButtonText = "Ok" });
                }
            }
        }

        async Task<Guid> GetPersonIdAsync()
        {
            var response = await GraphServiceClient.Me.Request().GetAsync();
            return Guid.Parse(response.Id);
        }

        bool _showForm = false;

        void ToggleForm()
        {
            _showForm = !_showForm;
            if (!_showForm)
            {
                ActivityRecordsModel.ProjectGUID = default;
                ActivityRecordsModel.ActivityGUID = default;
                ActivityRecordsModel.TypeOfWork = default;

                for (int i = 0; i < ActivityRecordsModel.Days.Length; i++)
                {
                    ActivityRecordsModel.Days[i].Hours = default;
                }
            }
        }

        public async Task OpenHistoryDialog()
        {
            var sidebarToggleValue = await JSRuntime.InvokeAsync<string>("eval", "localStorage.getItem('sb|sidebar-toggle')");

            bool isSidebarClosed = sidebarToggleValue == "true";

            if (!isSidebarClosed)
            {
                await JSRuntime.InvokeVoidAsync("toggleSidenavbar");
            }

            await DialogService.OpenAsync<TimesheetHistory>(_localizer["ViewHistory"],
            new Dictionary<string, object>()
            {
                    { "TimesheetData", TimesheetModel }

            }, new DialogOptions() { Width = "Auto" });
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
                    await DialogService.Alert(_localizer["NoActivitiesData"], _localizer["NoDataFound"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 500:
                    await DialogService.Alert(_localizer["PossiblyNoDrafts"], _localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
            }
        }
    }
}
