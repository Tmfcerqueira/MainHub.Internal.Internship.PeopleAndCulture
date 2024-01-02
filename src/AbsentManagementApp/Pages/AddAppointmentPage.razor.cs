using MainHub.Internal.PeopleAndCulture.App.Models;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.Graph;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models;
using AbsentManagement.Api.Proxy.Client.Client;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Pages
{
    public partial class AddAppointmentPage
    {

        [Inject]
        DialogService DialogService { get; set; } = null!;

        [Parameter]
        public DateTime AbsenceStart { get; set; }

        [Parameter]
        public DateTime AbsenceEnd { get; set; }

        [Inject]
        IAbsenceAppRepository AbsenceRepository { get; set; } = null!;

        //for person guid
        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;


        [Inject]
        private ProtectedLocalStorage ProtectedLocalStorage { get; set; } = null!;

        AbsenceModel _model = new();

        IEnumerable<AbsenceTypeModel> _types = null!;

        private bool _isLoading = true;

        protected override async Task OnInitializedAsync()
        {

            var result = await ProtectedLocalStorage.GetAsync<List<AbsenceTypeModel>>("types");

            if (result.Success && result.Value != null)
            {
                _types = result.Value;

            }
            else
            {
                _types = await AbsenceRepository.GetAllAbsenceTypesAsync();
            }

            if (_types.Any())
            {
                _model.AbsenceTypeGuid = _types.FirstOrDefault()?.TypeGuid ?? Guid.Empty;
            }
            else
            {
                // Handle the case when _types is empty
                _model.AbsenceTypeGuid = Guid.Empty;
                _model.AbsenceType = "Error, no types";
            }
        }

        protected override void OnParametersSet()
        {
            try
            {
                _model.AbsenceStart = AbsenceStart;
                _model.AbsenceEnd = AbsenceEnd;
            }
            finally
            {
                _isLoading = false;
            }
        }

        async Task OnSubmit(AbsenceModel model)
        {
            try
            {
                if (!await ValidateAbsenceDates(model))
                {
                    return;
                }

                if (!await ValidateAbsenceSchedule(model))
                {
                    return;
                }

                if (!await ValidateAbsenceType(model))
                {
                    return;
                }

                //set person id from logged user
                var personId = GraphServiceClient.Me.Request().GetAsync().Result.Id;

                model.PersonGuid = Guid.Parse(personId);
                model.ApprovalStatus = Common.ApprovalStatus.Submitted;

                var result = await AbsenceRepository.CreateAbsenceAsync(model, model.PersonGuid);


                if (result.Item2 == 200)
                {
                    // handle success

                    HandleSuccess(model);

                }
                else
                {
                    // handle error
                    await HandleError(result.Item2);
                }
            }
            catch (ApiException ex)
            {
                HandleError(ex.ErrorCode).RunSynchronously();
            }
            catch (Exception ex)
            {
                await DialogService.Alert(ex.Message, localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
            }
        }

        async Task SaveDraft(AbsenceModel model)
        {
            try
            {

                if (!await ValidateAbsenceDates(model))
                {
                    return;
                }

                if (!await ValidateAbsenceSchedule(model))
                {
                    return;
                }

                if (!await ValidateAbsenceType(model))
                {
                    return;
                }

                //set person id from logged user
                var personId = GraphServiceClient.Me.Request().GetAsync().Result.Id;

                model.PersonGuid = Guid.Parse(personId);

                var result = await AbsenceRepository.CreateAbsenceAsync(model, model.PersonGuid);


                if (result.Item2 == 200)
                {
                    // handle success

                    HandleSuccess(model);

                }
                else
                {
                    // handle error
                    await HandleError(result.Item2);
                }
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

        readonly List<string> _scheduleListTextFieldList = new()
        {
            "Full Day",
            "Morning Half Day",
            "Afternoon Half Day"
        };

        public void HandleSuccess(AbsenceModel model)
        {
            // close current form
            DialogService.Close(model);
            // handle success
            DialogService.Alert(localizer["AbsenceSaved"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
        }

        private async Task HandleError(int errorCode)
        {
            switch (errorCode)
            {
                case 400:
                    await DialogService.Alert(localizer["AlreadyAnAbsence"], localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 401:
                    await DialogService.Alert(localizer["YouAreNotAuthorized"], localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 404:
                    await DialogService.Alert(localizer["NoAbsenceData"], localizer["NoDataFound"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 500:
                    await DialogService.Alert(localizer["PossiblyThereAreNoDrafts"], localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
            }
        }
        async Task<bool> ValidateAbsenceDates(AbsenceModel model)
        {
            if (model.AbsenceStart > model.AbsenceEnd)
            {
                await DialogService.Alert(localizer["StartDateMust"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
                return false;
            }

            if (model.AbsenceStart <= DateTime.Now || model.AbsenceEnd <= DateTime.Now)
            {
                await DialogService.Alert(localizer["DateMustBeInFuture"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
                return false;
            }

            TimeSpan absenceDuration = model.AbsenceEnd - model.AbsenceStart;
            if (absenceDuration.TotalHours < 1)
            {
                await DialogService.Alert(localizer["WrongAbsenceInterval"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
                return false;
            }

            return true;
        }


        async Task<bool> ValidateAbsenceSchedule(AbsenceModel model)
        {
            if (model.Schedule == "Morning Half Day" && (model.AbsenceStart.Hour > 13 || model.AbsenceEnd.Hour > 13))
            {
                await DialogService.Alert(localizer["WrongHoursForMorning"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
                return false;
            }

            if (model.Schedule == "Afternoon Half Day" && (model.AbsenceStart.Hour < 13 || model.AbsenceEnd.Hour < 13))
            {
                await DialogService.Alert(localizer["WrongHoursForAfternoon"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
                return false;
            }

            TimeSpan absenceDuration = model.AbsenceEnd - model.AbsenceStart;
            if (model.Schedule == "Full Day" && absenceDuration.TotalHours < 6)
            {
                await DialogService.Alert(localizer["WrongHoursForFullDay"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
                return false;
            }

            return true;
        }


        async Task<bool> ValidateAbsenceType(AbsenceModel model)
        {
            if (model.AbsenceTypeGuid == Guid.Empty)
            {
                await DialogService.Alert(localizer["PleaseSelectType"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
                return false;
            }
            return true;
        }
    }
}
