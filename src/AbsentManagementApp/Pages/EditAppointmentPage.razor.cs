using System;
using System.Reflection;
using AbsentManagement.Api.Proxy.Client.Client;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Shared.HeaderAreaComponent;
using MainHub.Internal.PeopleAndCulture.App.Models;
using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Graph;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Pages
{
    public partial class EditAppointmentPage
    {
        [Inject]
        DialogService DialogService { get; set; } = null!;

        [Parameter]
        public AbsenceModel Absence { get; set; } = null!;

        AbsenceModel _model = new();

        //for person guid
        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Inject]
        IAbsenceAppRepository AbsenceRepository { get; set; } = null!;

        [Inject]
        private ProtectedLocalStorage ProtectedLocalStorage { get; set; } = null!;

        [Inject]
        TooltipService TooltipService { get; set; } = null!;


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
        }

        protected override void OnParametersSet()
        {
            try
            {
                _model = Absence;
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

                if (!await ValidateAbsenceSchedule(model))
                {
                    return;
                }

                if (!await ValidateAbsenceType(model))
                {
                    return;
                }

                var loggedPerson = await GetPersonIdAsync();

                // Call the UpdateAbsence method to update the absence
                var result = await AbsenceRepository.UpdateAbsenceAsync(model.PersonGuid, model.AbsenceGuid, model, loggedPerson);


                if (result)
                {
                    // handle success
                    HandleSuccess(model);
                }
                else
                {
                    // handle error
                    HandleError();
                }
            }
            catch (ApiException)
            {
                HandleError();
            }
            catch (Exception ex)
            {
                await DialogService.Alert(ex.Message, localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
            }
        }

        async Task<Guid> GetPersonIdAsync()
        {
            var response = await GraphServiceClient.Me.Request().GetAsync();
            return Guid.Parse(response.Id);
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

        private async Task DeleteItem()
        {
            try
            {
                bool confirmed = await DialogService.Confirm(localizer["Areyousure"], localizer["Delete"], new ConfirmOptions() { OkButtonText = localizer["Yes"], CancelButtonText = localizer["No"] }) ?? false;

                if (confirmed)
                {
                    //set person id from logged user
                    var personId = await GetPersonIdAsync();

                    //update isDeleted & DeletedBy field
                    var result = await AbsenceRepository.DeleteAbsenceAsync(personId, _model.AbsenceGuid, personId);

                    if (result)
                    {
                        // close current form
                        DialogService.Close();

                        // handle success
                        await DialogService.Alert(localizer["AbsenceDeleted"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });

                    }
                    else
                    {
                        // handle error
                        await DialogService.Alert(localizer["ErrorDeleting"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
                    }
                }
            }
            catch (Exception ex)
            {
                await DialogService.Alert(ex.Message, "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
            }
        }

        private async Task SubmitItem()
        {
            try
            {
                bool confirmed = await DialogService.Confirm(localizer["Areyousure"], localizer["Submit"], new ConfirmOptions() { OkButtonText = localizer["Yes"], CancelButtonText = localizer["No"] }) ?? false;

                if (confirmed)
                {
                    //set person id from logged user
                    var personId = await GetPersonIdAsync();

                    //update submit field
                    var subbmitedAbsence = _model;
                    subbmitedAbsence.SubmissionDate = DateTime.Now;
                    subbmitedAbsence.ApprovalStatus = Common.ApprovalStatus.Submitted;

                    var result = await AbsenceRepository.UpdateAbsenceAsync(subbmitedAbsence.PersonGuid, subbmitedAbsence.AbsenceGuid, subbmitedAbsence, personId);

                    if (result)
                    {
                        // close current form
                        DialogService.Close();

                        // handle success
                        await DialogService.Alert(localizer["AbsenceSubmitted"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });

                    }
                    else
                    {
                        // handle error
                        await DialogService.Alert(localizer["ErrorDeleting"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
                    }
                }
            }
            catch (Exception ex)
            {
                await DialogService.Alert(ex.Message, "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
            }
        }

        public async Task OpenHistoryDialog()
        {
            await DialogService.OpenAsync<HistoryDialog>(localizer["ViewHistory"],
            new Dictionary<string, object>()
            {
                    { "Absence", Absence }

            }, new DialogOptions() { Width = "80%" });
        }

        public void HandleSuccess(AbsenceModel model)
        {
            // close current form
            DialogService.Close(model);
            // handle success
            DialogService.Alert(localizer["AbsenceUpdated"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
        }

        public void HandleError()
        {
            // handle error
            DialogService.Alert(localizer["ErrorUpdating"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
        }

        readonly List<string> _scheduleListTextFieldList = new()
        {
            "Full Day",
            "Morning Half Day",
            "Afternoon Half Day"
        };


        void ShowHistoryTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, localizer["ActionHistory"], options);

        void ShowDeleteTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, localizer["DeleteAbsence"], options);

        void ShowSubmitTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, localizer["SubmitAbsence"], options);

    }
}
