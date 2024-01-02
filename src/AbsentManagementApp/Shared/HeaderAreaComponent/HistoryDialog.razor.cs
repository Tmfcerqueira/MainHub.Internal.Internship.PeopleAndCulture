using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Radzen;
using AbsentManagement.Api.Proxy.Client.Client;
using MainHub.Internal.PeopleAndCulture.App.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Pages;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Shared.HeaderAreaComponent
{
    public partial class HistoryDialog
    {
        [Inject]
        IAbsenceAppRepository AbsenceRepository { get; set; } = null!;

        [Inject]
        DialogService DialogService { get; set; } = null!;

        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Parameter]
        public AbsenceModel Absence { get; set; } = null!;

        private int PageSize { get; set; } = 5;
        private int Count { get; set; } = 0;

        bool _tableIsLoading = false;

        Guid PersonId { get; set; }

        private (List<AbsenceHistoryModel> absenceHistory, int errorCode, int count) _absenceHistory = (new List<AbsenceHistoryModel>(), 200, 0);

        private AbsenceHistoryModel? _selectedAbsence;

        protected override async Task OnInitializedAsync()
        {
            try
            {

                PersonId = await GetPersonIdAsync();


                LoadDataArgs args = new LoadDataArgs()
                {
                    Filter = "",
                    Skip = 0,
                    OrderBy = "",
                    Top = PageSize,
                };

                //history list

                await LoadHistoryData(args);
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

        async Task LoadHistoryData(LoadDataArgs args)
        {

            try
            {
                _tableIsLoading = true;

                var skip = args.Skip ?? 0;
                var page = (int)(skip / PageSize) + 1;
                var filter = args.Filter;

                _absenceHistory = await AbsenceRepository.GetAbsenceHistory(Absence.AbsenceGuid, PersonId.ToString(), page, PageSize);


                Count = _absenceHistory.count;


                _tableIsLoading = false;
            }
            catch (ApiException ex)
            {
                await HandleError(ex.ErrorCode);
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
                    _absenceHistory.absenceHistory = new List<AbsenceHistoryModel>();
                    await DialogService.Alert(localizer["NoAbsenceData"], localizer["NoDataFound"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
                case 500:
                    await DialogService.Alert("Possibly there are no drafts. Please try adding some", localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
                    break;
            }
        }



        async Task<Guid> GetPersonIdAsync()
        {
            var response = await GraphServiceClient.Me.Request().GetAsync();
            return Guid.Parse(response.Id);
        }
    }
}
