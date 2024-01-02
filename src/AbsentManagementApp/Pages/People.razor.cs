using AbsentManagement.Api.Proxy.Client.Client;
using App.Models;
using App.Repository;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models;
using MainHub.Internal.PeopleAndCulture.App.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Microsoft.JSInterop;
using Radzen;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Pages
{
    public partial class People
    {
        [Inject]
        IAbsenceAppRepository AbsenceRepository { get; set; } = null!;


        [Inject]
        DialogService DialogService { get; set; } = null!;


        private (List<AllPeopleModel> collaborators, int count) _collaborators;

        private AllPeopleModel? SelectedPerson { get; set; } = null!;


        bool _isLoading = false;

        int PageSize { get; set; } = 8;
        int Count { get; set; }
        private string Filter { get; set; } = null!;
        bool _isFiltered = false;
        State _listfilter = State.Active;
        private int Page { get; set; }


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
                Page = (int)(skip / PageSize) + 1;
                var filter = args.Filter;

                _collaborators = await AbsenceRepository.GetAllPeople(Page, PageSize, null!, _listfilter);

                Count = _collaborators.count;
                _isLoading = false;


            }
            catch (Exception ex)
            {
                await DialogService.Alert(ex.Message, localizer["Error"], new AlertOptions() { OkButtonText = "Ok" });
            }

        }

        public async Task Search(string filter)
        {
            PageSize = 10;
            if (_isFiltered == false)
            {
                _isLoading = true;
                _collaborators = await AbsenceRepository.GetAllPeople(Page, PageSize, filter, _listfilter);
                Count = _collaborators.count;
                _isLoading = false;
                _isFiltered = true;
            }
            else if (_isFiltered == true)
            {
                _isLoading = true;
                _collaborators = await AbsenceRepository.GetAllPeople(Page, PageSize, null!, _listfilter);
                Count = _collaborators.count;
                _isLoading = false;
                _isFiltered = false;
            }
        }

        private async Task ShowActive(State list)
        {
            _isLoading = true;
            _listfilter = list;
            _collaborators = await AbsenceRepository.GetAllPeople(Page, PageSize, null!, _listfilter);
            Count = _collaborators.count;
            _isLoading = false;
        }

        public async Task ShowCollaborator(Guid id)
        {
            try
            {
                SelectedPerson = _collaborators.collaborators.FirstOrDefault(a => a.PeopleGUID == id);

                if (SelectedPerson != null)
                {
                    await DialogService.OpenAsync<AdminUserCalendar>($"{SelectedPerson.FirstName} {SelectedPerson.LastName} Info",
                                    new Dictionary<string, object> { { "PersonGuid", SelectedPerson.PeopleGUID } },
                                    new DialogOptions { Width = "auto", Style = "margin-top:3.5%; min-width: calc(100vw - 300px); max-height: calc(100vh - 170);max-width: 80%; overflow-y: auto;" });
                }
            }
            catch (Exception ex)
            {
                await dialogService.Alert(localizer["NoUserInfo"], localizer["NoInformation"], new AlertOptions() { OkButtonText = "Ok" });
                Console.WriteLine(ex);
                NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
            }
        }

    }
}
