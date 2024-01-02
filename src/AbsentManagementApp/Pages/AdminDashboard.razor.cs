using MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Radzen;
using MainHub.Internal.PeopleAndCulture.App.Models;
using System.Drawing.Printing;
using AbsentManagement.Api.Proxy.Client.Model;
using AbsentManagement.Api.Proxy.Client.Client;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Pages
{
    public partial class AdminDashboard
    {
        [Inject]
        IAbsenceAppRepository AbsenceRepository { get; set; } = null!;

        [Inject]
        DialogService DialogService { get; set; } = null!;

        [Inject]
        NavigationManager NavigationManager { get; set; } = null!;
        private int Approved { get; set; } = 0;
        private int Draft { get; set; } = 0;
        private int Submitted { get; set; } = 0;
        private int Rejected { get; set; } = 0;

        bool _isLoading = false;

        private (List<AbsenceModel> absences, int errorCode, int count) _absences = (new List<AbsenceModel>(), 200, 0);


        protected override async Task OnInitializedAsync()
        {
            try
            {
                _isLoading = true;

                var result = (new List<AbsenceModel>(), 200, 0);
                int resultCount;
                var count = 0;
                var pageSize = 20;
                var page = 1;

                result = await AbsenceRepository.GetAllAbsencesAsync(page, pageSize, ApprovalStatus.All);
                resultCount = result.Item3;
                _absences.absences.AddRange(result.Item1);
                count += result.Item1.Count;
                while (count < resultCount)
                {
                    page++;
                    result = await AbsenceRepository.GetAllAbsencesAsync(page, pageSize, ApprovalStatus.All);
                    _absences.absences.AddRange(result.Item1);
                    count += result.Item1.Count;
                }

                _absences.absences.ForEach(a =>
                {
                    switch (a.ApprovalStatus)
                    {
                        case Common.ApprovalStatus.Approved: Approved++; break;
                        case Common.ApprovalStatus.Draft: Draft++; break;
                        case Common.ApprovalStatus.Submitted: Submitted++; break;
                        case Common.ApprovalStatus.Rejected: Rejected++; break;
                    }
                });

                _isLoading = false;
            }
            catch (ApiException ex)
            {
                await HandleError(ex.ErrorCode);
            }
            catch (Exception)
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
            }
        }

        private void RedirectToApproval()
        {
            NavigationManager.NavigateTo("/approval");
        }

        private void RedirectToPeople()
        {
            NavigationManager.NavigateTo("/people");
        }

        private void RedirectToCalendar()
        {
            NavigationManager.NavigateTo("/alluserscalendar");
        }
    }
}
