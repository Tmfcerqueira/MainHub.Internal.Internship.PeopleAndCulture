using AbsentManagement.Api.Proxy.Client.Client;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Shared.HeaderAreaComponent;
using MainHub.Internal.PeopleAndCulture.App.Models;
using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;


namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Pages
{
    public partial class AbsenceDetailsDialog
    {
        [Inject]
        DialogService DialogService { get; set; } = null!;

        [Parameter]
        public AbsenceModel Absence { get; set; } = null!;

        [Inject]
        TooltipService TooltipService { get; set; } = null!;


        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Inject]
        IAbsenceAppRepository AbsenceRepository { get; set; } = null!;

        private bool _isLoading;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _isLoading = true;

                if (Absence == null)
                {
                    await HandleError(404);
                }
            }
            catch (ApiException ex)
            {
                await HandleError(ex.ErrorCode);
            }
            catch (Exception ex)
            {
                await DialogService.Alert($"{localizer["UnknownError"]}:\n" + ex.Message, localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
            }
            finally
            {
                _isLoading = false;
            }
        }


        private async Task CancelItem()
        {
            try
            {
                bool confirmed = await DialogService.Confirm(localizer["Areyousure"], localizer["Cancel"], new ConfirmOptions() { OkButtonText = localizer["Yes"], CancelButtonText = localizer["No"] }) ?? false;

                if (confirmed)
                {
                    _isLoading = true;

                    //set person id from logged user
                    Guid personId = await GetPersonIdAsync();

                    var absence = Absence;

                    if (absence != null)
                    {

                        // Call the UpdateAbsence method to update the absence status
                        absence.ApprovalStatus = ApprovalStatus.Draft;

                        var result = await AbsenceRepository.UpdateAbsenceAsync(absence.PersonGuid, absence.AbsenceGuid, absence, personId);

                        if (result)
                        {
                            // handle success
                            await DialogService.Alert(localizer["AbsenceCanceleted"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });

                            // Reload 
                            await OnInitializedAsync();
                        }
                        else
                        {
                            // handle error
                            await DialogService.Alert(localizer["ErrorCanceling"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
                        }
                    }
                    else
                    {
                        await HandleError(404);
                    }
                    _isLoading = false;
                }

            }
            catch (ApiException ex)
            {
                await HandleError(ex.ErrorCode);
            }
            catch (Exception ex)
            {
                await DialogService.Alert(ex.Message, "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
            }
        }


        private async Task RejectItem()
        {
            try
            {
                bool confirmed = await DialogService.Confirm(localizer["Areyousure"], localizer["Reject"], new ConfirmOptions() { OkButtonText = localizer["Yes"], CancelButtonText = localizer["No"] }) ?? false;

                if (confirmed)
                {
                    _isLoading = true;

                    //set person id from logged user
                    Guid personId = await GetPersonIdAsync();

                    var absence = Absence;

                    if (absence != null)
                    {

                        // Call the UpdateAbsence method to update the absence status
                        absence.ApprovalStatus = ApprovalStatus.Rejected;

                        var result = await AbsenceRepository.UpdateAbsenceAsync(absence.PersonGuid, absence.AbsenceGuid, absence, personId);

                        if (result)
                        {
                            // handle success
                            await DialogService.Alert(localizer["AbsenceRejected"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });

                            // Reload 
                            await OnInitializedAsync();
                        }
                        else
                        {
                            // handle error
                            await DialogService.Alert(localizer["ErrorRejecting"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
                        }
                    }
                    else
                    {
                        await HandleError(404);
                    }
                    _isLoading = false;
                }

            }
            catch (ApiException ex)
            {
                await HandleError(ex.ErrorCode);
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
                    await DialogService.Alert(localizer["TheRequestWasInvalid"], localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 401:
                    await DialogService.Alert(localizer["YouAreNotAuthorized"], localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 404:
                    await DialogService.Alert(localizer["NoAbsenceData"], localizer["NoDataFound"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
            }
        }




        void ShowTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, localizer["ActionHistory"], options);

        void ShowCancelTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, localizer["CancelAbsenceToDraft"], options);

        void ShowRejectTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, localizer["RejectAbsence"], options);
    }
}
