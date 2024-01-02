using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Radzen.Blazor;
using Radzen;
using global::AbsentManagement.Api.Proxy.Client.Model;
using System;
using System.Collections.Generic;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models;
using MainHub.Internal.PeopleAndCulture.App.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Drawing.Printing;
using AbsentManagement.Api.Proxy.Client.Client;
using Microsoft.Graph.SecurityNamespace;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Pages
{
    public partial class Dashboard
    {
        [Inject]
        IAbsenceAppRepository AbsenceRepository { get; set; } = null!;

        [Inject]
        DialogService DialogService { get; set; } = null!;

        [Inject]
        private ProtectedLocalStorage ProtectedLocalStorage { get; set; } = null!;

        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Inject]
        NavigationManager NavigationManager { get; set; } = null!;

        private int PageSize { get; set; } = 4;
        private int Count { get; set; } = 0;

        private int Approved { get; set; } = 0;
        private int Draft { get; set; } = 0;
        private int Submitted { get; set; } = 0;
        private int Rejected { get; set; } = 0;

        Guid PersonId { get; set; }

        bool _isLoading = false;

        bool _tableIsLoading = false;

        private (List<AbsenceModel> absences, int errorCode, int count) _approvedAbsences = (new List<AbsenceModel>(), 200, 0);

        private (List<AbsenceModel> absences, int errorCode, int count) _absences = (new List<AbsenceModel>(), 200, 0);

        private AbsenceModel? _selectedAbsence;

        protected override async Task OnInitializedAsync()
        {
            try
            {

                _isLoading = true;

                PersonId = await GetPersonIdAsync();

                List<AbsenceModel> resultAppList = new List<AbsenceModel>();
                var result = (new List<AbsenceModel>(), 200, 0);
                int resultCount;
                var count = 0;
                var pageSize = 20;
                var page = 1;

                result = await AbsenceRepository.GetAbsencesByPersonAsync(PersonId, DateTime.Now.Year, page, pageSize, ApprovalStatus.All, DateTime.Now, DateTime.Now.AddDays(8));//1 week absences of 1 person
                resultCount = result.Item3;
                resultAppList.AddRange(result.Item1);
                count += result.Item1.Count;
                while (count < resultCount)
                {
                    page++;
                    result = await AbsenceRepository.GetAbsencesByPersonAsync(PersonId, DateTime.Now.Year, page, pageSize, ApprovalStatus.All, DateTime.Now, DateTime.Now.AddDays(8));//1 week absences of 1 person
                    resultAppList.AddRange(result.Item1);
                    count += result.Item1.Count;
                }

                resultAppList.ForEach(a =>
                {
                    switch (a.ApprovalStatus)
                    {
                        case Common.ApprovalStatus.Approved: Approved++; break;
                        case Common.ApprovalStatus.Draft: Draft++; break;
                        case Common.ApprovalStatus.Submitted: Submitted++; break;
                        case Common.ApprovalStatus.Rejected: Rejected++; break;
                    }
                });


                LoadDataArgs args = new LoadDataArgs()
                {
                    Filter = "",
                    Skip = 0,
                    OrderBy = "",
                    Top = PageSize,
                };

                await LoadApprovedData(args);
                _absences.absences = resultAppList;
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

        async Task LoadApprovedData(LoadDataArgs args)
        {

            try
            {
                _tableIsLoading = true;

                var types = await AbsenceRepository.GetAllAbsenceTypesAsync();

                types = types.OrderBy(type => type.Type).ToList();

                await ProtectedLocalStorage.SetAsync("types", types);

                var skip = args.Skip ?? 0;
                var page = (int)(skip / PageSize) + 1;
                var filter = args.Filter;

                _approvedAbsences = await AbsenceRepository.GetAbsencesByPersonAsync(PersonId, DateTime.Now.Year, page, PageSize, ApprovalStatus.Approved, DateTime.Now, DateTime.Now.AddDays(8));

                _approvedAbsences.absences = _approvedAbsences.absences.OrderBy(a => a.AbsenceStart).ToList();

                Count = _approvedAbsences.count;


                _tableIsLoading = false;
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

        private async Task ShowAbsence(Guid absenceGuid)
        {
            _selectedAbsence = _approvedAbsences.absences.FirstOrDefault(a => a.AbsenceGuid == absenceGuid);

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
                    _approvedAbsences.absences = new List<AbsenceModel>();
                    await DialogService.Alert(localizer["NoAbsenceData"], localizer["NoDataFound"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 500:
                    await DialogService.Alert(localizer["PossiblyThereAreNoDrafts"], localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
            }
        }

        private void RedirectToCalendar()
        {
            NavigationManager.NavigateTo("/calendar");
        }
        private void RedirectToRequest()
        {
            NavigationManager.NavigateTo("/request");
        }
        private void RedirectToListView()
        {
            NavigationManager.NavigateTo("/absences");
        }

    }
}
