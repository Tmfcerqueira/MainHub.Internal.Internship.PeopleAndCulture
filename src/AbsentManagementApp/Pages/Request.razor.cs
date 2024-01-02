using MainHub.Internal.PeopleAndCulture.App.Models;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.Graph;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models;
using ProxyModel = AbsentManagement.Api.Proxy.Client.Model;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using AbsentManagement.Api.Proxy.Client.Client;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Pages
{
    public partial class Request
    {
        [Inject]
        DialogService DialogService { get; set; } = null!;

        //for person guid
        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Inject]
        IAbsenceAppRepository AbsenceRepository { get; set; } = null!;

        bool IsLoading { get; set; } = false;

        [Inject]
        private ProtectedLocalStorage ProtectedLocalStorage { get; set; } = null!;

        readonly AbsenceModel _model = new AbsenceModel();

        IEnumerable<AbsenceTypeModel> _types = new List<AbsenceTypeModel>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await base.OnInitializedAsync();

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
            catch (ApiException ex)
            {
                await HandleError(ex.ErrorCode);
            }
            catch (Exception ex)
            {
                await DialogService.Alert(ex.Message, localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
            }
        }


        readonly List<string> _scheduleListTextFieldList = new List<string>
        {
            "Full Day",
            "Morning Half Day",
            "Afternoon Half Day"
        };

        async Task OnSubmit(AbsenceModel model)
        {
            try
            {
                IsLoading = true;

                var personId = (await GraphServiceClient.Me.Request().GetAsync()).Id;
                model.PersonGuid = Guid.Parse(personId);

                if (!await ValidateAbsenceDates(model))
                {
                    IsLoading = false;
                    return;
                }

                if (!await ValidateAbsenceSchedule(model))
                {
                    IsLoading = false;
                    return;
                }

                if (!await ValidateAbsenceType(model))
                {
                    IsLoading = false;
                    return;
                }

                model.ApprovalStatus = Common.ApprovalStatus.Submitted;
                model.SubmissionDate = DateTime.Now;
                model.ApprovedBy = "None";

                var result = await AbsenceRepository.CreateAbsenceAsync(model, model.PersonGuid);

                IsLoading = false;

                if (result.Item2 == 200)
                {
                    // handle success
                    await DialogService.Alert(localizer["AbsenceSaved"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
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

        async Task SaveDraft(AbsenceModel model)
        {
            try
            {
                IsLoading = true;

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

                IsLoading = false;

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

        public void HandleSuccess(AbsenceModel model)
        {
            // close current form
            DialogService.Close(model);
            // handle success
            DialogService.Alert(localizer["AbsenceSaved"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
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
                    await DialogService.Alert(localizer["ErrorProcessing"], localizer["ServerError"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
            }
        }




    }
}
