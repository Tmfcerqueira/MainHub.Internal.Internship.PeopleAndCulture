using System.Linq.Dynamic.Core.Tokenizer;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Graph;
using Radzen;
using Proxy = TimesheetManagement.Api.Proxy.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Components.ActivityComponent;
using Microsoft.JSInterop;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.Pages
{
    public partial class TimesheetsPage
    {
        [Inject]
        ITimesheetAppRepository TimesheetRepository { get; set; } = null!;

        //for person guid
        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Inject]
        DialogService DialogService { get; set; } = null!;

        [Inject]
        NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        TooltipService TooltipService { get; set; } = null!;

        private (List<TimesheetModel> timesheets, int errorCode, int count) _timesheets = (new List<TimesheetModel>(), 200, 0);

        readonly TimesheetModel _timesheetModel = new TimesheetModel()
        {
            Month = DateTime.Now.Month,
            Year = DateTime.Now.Year
        };

        string _help = "";

        Guid PersonGuid { get; set; }

        private bool IsLoading { get; set; } = false;

        readonly int _currentYear = DateTime.Now.Year;
        readonly int _currentMonth = DateTime.Now.Month;
        readonly string _currentMonthName = DateTime.Now.ToString("MMMM", CultureInfo.GetCultureInfo("en-US"));

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            PersonGuid = await GetPersonIdAsync();

            _help = _localizer["ToolTip"];

            //Get all timesheets by person
            _timesheets = await TimesheetRepository.GetAllTimesheetsFromOnePerson(PersonGuid, 0, 0, Proxy.ApprovalStatus.All, 1, 20);

            IsLoading = false;
        }

        async void OnCreate(TimesheetModel model)
        {
            if (_timesheets.timesheets != null && _timesheets.timesheets.Any(a => a.Month == _currentMonth && a.Year == _currentYear))
            {
                await DialogService.Alert(@_localizer["TimesheetAlreadyExists"], @_localizer["CreateTimesheet"], new AlertOptions() { OkButtonText = "OK" });
            }
            else
            {
                model.PersonGUID = PersonGuid;
                await TimesheetRepository.CreateTimesheet(model, PersonGuid);
                await DialogService.Alert(@_localizer["TimesheetCreated"], @_localizer["CreateTimesheet"], new AlertOptions() { OkButtonText = "OK" });
                //Reload
                NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
            }
        }

        bool _showForm = false;

        void ToggleForm()
        {
            _showForm = !_showForm;
        }

        private async Task OnRowDoubleClick(Guid timesheetGuid, Guid personGuid)
        {
            NavigationManager.NavigateTo($"/activities/{timesheetGuid}/{personGuid}");
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

        void ShowHelpTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, _help, options);
    }
}
