using System;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models;
using MainHub.Internal.PeopleAndCulture.App.Models;
using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Radzen;
using ProxyModel = AbsentManagement.Api.Proxy.Client.Model;
using Radzen.Blazor;
using AbsentManagement.Api.Proxy.Client.Client;
using Microsoft.JSInterop;
using System.Reflection;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Pages
{
    public partial class Approval
    {
        [Inject]
        IAbsenceAppRepository AbsenceRepository { get; set; } = null!;

        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Inject]
        DialogService DialogService { get; set; } = null!;

        [Inject]
        NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        TooltipService TooltipService { get; set; } = null!;

        private bool _isLoading;

        private int PageSize { get; set; } = 6;

        private int Count { get; set; } = 0;

        private AbsenceModel? _selectedAbsence;

        private bool _pageLoading;


        private (List<AbsenceModel> absences, int errorCode, int count) _absences = (new List<AbsenceModel>(), 200, 0);

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

                await LoadData(args);
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

        async Task LoadData(LoadDataArgs args)
        {
            try
            {


                _isLoading = true;

                var skip = args.Skip ?? 0;
                var page = (int)(skip / PageSize) + 1;

                _absences = await AbsenceRepository.GetAllAbsencesAsync(page, PageSize, ProxyModel.ApprovalStatus.Submitted);

                Count = _absences.count;

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

        async Task<Guid> GetPersonIdAsync()
        {
            var response = await GraphServiceClient.Me.Request().GetAsync();
            return Guid.Parse(response.Id);
        }



        private async Task RejectItem(Guid absenceId)
        {
            try
            {
                bool confirmed = await DialogService.Confirm(localizer["Areyousure"], localizer["Reject"], new ConfirmOptions() { OkButtonText = localizer["Yes"], CancelButtonText = localizer["No"] }) ?? false;

                if (confirmed)
                {
                    _isLoading = true;

                    //set person id from logged user
                    Guid personId = await GetPersonIdAsync();

                    var absence = _absences.absences.FirstOrDefault(a => a.AbsenceGuid == absenceId);

                    if (absence != null)
                    {

                        // Call the UpdateAbsence method to update the absence status
                        absence.ApprovedBy = "None";
                        absence.ApprovalStatus = ApprovalStatus.Rejected;

                        var result = await AbsenceRepository.UpdateAbsenceAsync(absence.PersonGuid, absence.AbsenceGuid, absence, personId);

                        if (result)
                        {
                            // handle success
                            await DialogService.Alert(localizer["AbsenceRejected"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });

                            // Reload 
                            StateHasChanged();
                            _pageLoading = true;
                            await LoadData(new LoadDataArgs());
                            _pageLoading = false;

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

        private async Task ApproveItem(Guid absenceGuid)
        {
            try
            {
                bool confirmed = await DialogService.Confirm(localizer["Areyousure"], localizer["Approve"], new ConfirmOptions() { OkButtonText = localizer["Yes"], CancelButtonText = localizer["No"] }) ?? false;

                if (confirmed)
                {

                    //set person id from logged user
                    Guid personId = await GetPersonIdAsync();

                    var absence = _absences.absences.FirstOrDefault(a => a.AbsenceGuid == absenceGuid);

                    if (absence != null)
                    {
                        absence.ApprovalStatus = ApprovalStatus.Approved;
                        absence.ApprovalDate = DateTime.Now;
                        absence.ApprovedBy = personId.ToString();

                        var result = await AbsenceRepository.UpdateAbsenceAsync(absence.PersonGuid, absence.AbsenceGuid, absence, personId);

                        if (result)
                        {
                            // handle success
                            await DialogService.Alert(localizer["AbsenceApproved"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });

                            // Reload 
                            StateHasChanged();
                            _pageLoading = true;
                            await LoadData(new LoadDataArgs());
                            _pageLoading = false;
                        }
                        else
                        {
                            // handle error
                            await DialogService.Alert(localizer["ErrorApproving"], "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });
                        }

                    }
                    else
                    {
                        await HandleError(404);
                    }
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

        private async Task ShowAbsence(Guid absenceGuid)
        {
            _selectedAbsence = _absences.absences.FirstOrDefault(a => a.AbsenceGuid == absenceGuid);

            if (_selectedAbsence != null)
            {
                await DialogService.OpenAsync<AbsenceDetailsDialog>(localizer["AbsenceDetails"], new Dictionary<string, object>
                {
                    { "Absence", _selectedAbsence }
                });
            }
            else
            {
                await HandleError(404);
            }
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

        void ShowApproveTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, localizer["Approve"], options);

        void ShowRejectTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, localizer["Reject"], options);
    }
}

