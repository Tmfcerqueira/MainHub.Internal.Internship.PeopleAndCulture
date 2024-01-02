using MainHub.Internal.PeopleAndCulture.App.Models;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using Microsoft.Graph;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models;
using MainHub.Internal.PeopleAndCulture.Common;
using ProxyModel = AbsentManagement.Api.Proxy.Client.Model;
using System.Drawing.Printing;
using AbsentManagement.Api.Proxy.Client.Client;
using System.Globalization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Pages
{
    public partial class Calendar
    {
        [Inject]
        IAbsenceAppRepository AbsenceRepository { get; set; } = null!;

        //for person guid
        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Inject]
        DialogService DialogService { get; set; } = null!;

        [Inject]
        TooltipService TooltipService { get; set; } = null!;

        bool _isLoading = true;
        RadzenScheduler<AbsenceModel> _scheduler = new();
        readonly Dictionary<DateTime, string> _events = new();
        private (List<AbsenceModel> absences, int errorCode, int count) _absences = (new List<AbsenceModel>(), 200, 0);
        readonly CultureInfo _culture = new("en-US");
        Guid _personGuid = Guid.Empty;
        string _help = "";


        protected override async Task OnInitializedAsync()
        {
            try
            {
                _help = localizer["CalendarHelp"];

                _isLoading = false;

            }
            catch (ApiException ex)
            {
                await HandleError(ex.ErrorCode);
            }
            catch (Exception ex)
            {
                await DialogService.Alert(ex.Message, localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
            }
        }


        async Task LoadData(SchedulerLoadDataEventArgs args)
        {
            try
            {
                _personGuid = Guid.Parse(GraphServiceClient.Me.Request().GetAsync().Result.Id);

                var result = (new List<AbsenceModel>(), 200, 0);
                int resultCount;
                var count = 0;
                List<AbsenceModel> resultAppList = new List<AbsenceModel>();
                var pageSize = 20;
                var page = 1;

                result = await AbsenceRepository.GetAbsencesByPersonAsync(_personGuid, DateTime.Now.Year, page, pageSize, ProxyModel.ApprovalStatus.All, new DateTime(), new DateTime());
                resultCount = result.Item3;
                resultAppList.AddRange(result.Item1);
                count += result.Item1.Count;
                while (count < resultCount)
                {
                    page++;
                    result = await AbsenceRepository.GetAbsencesByPersonAsync(_personGuid, DateTime.Now.Year, page, pageSize, ProxyModel.ApprovalStatus.All, new DateTime(), new DateTime());
                    resultAppList.AddRange(result.Item1);
                    count += result.Item1.Count;
                }


                if (_absences.absences.Count <= 0)
                {
                    await HandleError(_absences.errorCode);
                }

                _absences.absences = resultAppList;

            }
            catch (ApiException ex)
            {
                await HandleError(ex.ErrorCode);
                _isLoading = false;
            }
            catch (Exception ex)
            {
                await DialogService.Alert(ex.Message, localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                _isLoading = false;
            }
        }




        void OnSlotRender(SchedulerSlotRenderEventArgs args)
        {

            // Highlight today in month view
            if (args.View.Text == "Month" && args.Start.Date == DateTime.Today)
            {
                args.Attributes["style"] = "background: rgba(255,220,40,.2);";
            }

            // Highlight working hours (9-18)
            if ((args.View.Text == "Week" || args.View.Text == "Day") && args.Start.Hour > 8 && args.Start.Hour <= 17)
            {
                args.Attributes["style"] = "background: rgba(255,220,40,.2);";
            }

        }

        async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            args.End = args.Start;

            Console.WriteLine($"SlotSelect: Start={args.Start} End={args.End}");

            // Get the year from the selected date
            int year = args.Start.Year;

            // Compare the year with the current year
            if (year != DateTime.Now.Year)
            {
                await DialogService.Alert(localizer["OnlyAcceptedFromThisYear"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
                return; // Exit the method without opening the dialog
            }

            var data = await OpenAddAppointmentDialogAsync(args.Start, args.End);

            if (data != null)
            {
                AddAbsence(data);

                await _scheduler.Reload();
            }
        }


        void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<AbsenceModel> args)
        {
            // Never call StateHasChanged in AppointmentRender - would lead to infinite loop

            switch (args.Data.ApprovalStatus)
            {
                case ApprovalStatus.Draft: args.Attributes["style"] = "background: orange"; break;
                case ApprovalStatus.Approved: args.Attributes["style"] = "background: green"; break;
                case ApprovalStatus.Rejected: args.Attributes["style"] = "background: red"; break;
            }

        }

        //aux methods
        async Task<AbsenceModel> OpenAddAppointmentDialogAsync(DateTime start, DateTime end)
        {
            return await DialogService.OpenAsync<AddAppointmentPage>("Add Absence",
                new Dictionary<string, object> { { "AbsenceStart", start }, { "AbsenceEnd", end } },
                new DialogOptions { Width = "auto" });
        }

        private async Task HandleError(int errorCode)
        {
            switch (errorCode)
            {
                case 400:
                    await DialogService.Alert(localizer["TheRequestWasInvalid"], localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 401:
                    await DialogService.Alert(localizer["YouAreNotAuthorized"], localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 404:
                    _absences.absences = new List<AbsenceModel>();
                    await DialogService.Alert(localizer["NoAbsenceData"], localizer["NoDataFound"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 500:
                    await DialogService.Alert(localizer["PossiblyThereAreNoDrafts"], localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
            }
        }


        async Task SubmitAllDraft()
        {
            try
            {
                //set person id from logged user
                Guid personId = Guid.Parse(GraphServiceClient.Me.Request().GetAsync().Result.Id);

                var result = await AbsenceRepository.SubmitAllDraftAsync(personId, personId);

                if (result)
                {
                    await DialogService.Alert($"{localizer["AllDraftsSubmitted"]} 🙃", "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });

                    await _scheduler.Reload();
                }
                else
                {
                    await HandleError(500);
                }
            }
            catch (ApiException ex)
            {
                await HandleError(ex.ErrorCode);
            }
        }

        void AddAbsence(AbsenceModel data)
        {
            _absences.absences.Add(data);
        }

        async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<AbsenceModel> args)
        {

            string title = localizer["EditAbsence"];


            if (args.Data.ApprovalStatus != ApprovalStatus.Draft && args.Data.ApprovalStatus != ApprovalStatus.Rejected)
            {
                title = localizer["ViewAbsence"];
            }

            await DialogService.OpenAsync<EditAppointmentPage>(title, new Dictionary<string, object> { { "Absence", args.Data } }, new DialogOptions()
            {
                Width = "auto"
            });



            await _scheduler.Reload();
        }

        void ShowHelpTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, _help, options);
    }
}
