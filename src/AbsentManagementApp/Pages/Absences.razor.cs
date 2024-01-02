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
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Reflection;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Pages
{
    public partial class Absences
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

        private bool _pageLoading;

        private int PageSize { get; set; } = 7;

        private int Count { get; set; } = 0;

        private AbsenceModel? _selectedAbsence;


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
                    Top = PageSize
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

                var personId = await GetPersonIdAsync();

                var skip = args.Skip ?? 0;
                var page = (int)(skip / PageSize) + 1;
                var filter = args.Filter;

                ProxyModel.ApprovalStatus status = ParseApprovalStatus(filter);

                _absences = await AbsenceRepository.GetAbsencesByPersonAsync(personId, DateTime.Now.Year, page, PageSize, status, new DateTime(), new DateTime());

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

        ProxyModel.ApprovalStatus ParseApprovalStatus(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return ProxyModel.ApprovalStatus.All;
            }


            var parts = filter.Trim().Split('=');
            if (parts.Length == 2 && parts[0].Trim() == "ApprovalStatus")
            {
                string statusInNumber = parts[1].Trim();
                switch (statusInNumber)
                {
                    case "1":
                        return ProxyModel.ApprovalStatus.Draft;
                    case "2":
                        return ProxyModel.ApprovalStatus.Submitted;
                    case "3":
                        return ProxyModel.ApprovalStatus.Approved;
                    case "4":
                        return ProxyModel.ApprovalStatus.Rejected;
                    case "5":
                    default:
                        return ProxyModel.ApprovalStatus.All;
                }
            }

            return ProxyModel.ApprovalStatus.All;
        }

        private async Task EditItem(Guid absenceGuid)
        {
            try
            {

                _selectedAbsence = _absences.absences.FirstOrDefault(a => a.AbsenceGuid == absenceGuid);

                var result = await DialogService.OpenAsync<EditAppointmentPage>(localizer["EditAbsence"], new Dictionary<string, object> { { "Absence", _selectedAbsence } }, new DialogOptions()
                {
                    Width = "auto"
                });

                if (result != null)
                {
                    _pageLoading = true;
                    await LoadData(new LoadDataArgs());
                    _pageLoading = false;
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
                if (_selectedAbsence.ApprovalStatus == ApprovalStatus.Rejected || _selectedAbsence.ApprovalStatus == ApprovalStatus.Draft)
                {
                    await EditItem(_selectedAbsence.AbsenceGuid);
                }
                else
                {
                    await DialogService.OpenAsync<AbsenceDetailsDialog>(localizer["AbsenceDetails"], new Dictionary<string, object>
                    {
                        { "Absence", _selectedAbsence }
                    });
                }
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


        async Task SubmitAllDraft()
        {
            try
            {
                //set person id from logged user
                Guid personId = Guid.Parse(GraphServiceClient.Me.Request().GetAsync().Result.Id);

                var result = await AbsenceRepository.SubmitAllDraftAsync(personId, personId);

                if (result)
                {
                    await DialogService.Alert("All drafts submitted 🙃", "SkillHub Info", new AlertOptions() { OkButtonText = "Ok" });

                    // Reload the current page
                    _pageLoading = true;
                    await LoadData(new LoadDataArgs());
                    _pageLoading = false;
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


        void RowRender(RowRenderEventArgs<AbsenceModel> args)
        {
            switch (args.Data.ApprovalStatus)
            {
                case ApprovalStatus.Approved:
                    args.Attributes.Add("style", "background-color: rgba(197, 255, 189, 0.8);");
                    break;
                case ApprovalStatus.Rejected:
                    args.Attributes.Add("style", "background-color: rgba(255, 168, 168, 0.8);");
                    break;
                case ApprovalStatus.Draft:
                    args.Attributes.Add("style", "background-color: rgba(252, 252, 239, 0.8);");
                    break;
            }
        }

        void ShowEditTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Edit Absence", options);

        private void RedirectToRequest()
        {
            NavigationManager.NavigateTo("/request");
        }

    }
}
