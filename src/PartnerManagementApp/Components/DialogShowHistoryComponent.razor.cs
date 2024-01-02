using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PartnerManagement.Api.Proxy.Client.Client;
using PartnerManagement.App.Models;
using PartnerManagement.App.Repository;
using Radzen;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.Components
{
    [Authorize(Policy = "Supervisor")]
    public partial class DialogShowHistoryComponent
    {
        [Inject]
        private PartnerAppRepository PartnerRepository { get; set; } = null!;

        [Inject]
        private DialogService DialogService { get; set; } = null!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        [Parameter]
        public PartnerModel Partner { get; set; } = null!;
        [Parameter]
        public ContactModel Contact { get; set; } = null!;
        private bool IsLoading { get; set; }
        private int Count { get; set; }
        [Parameter]
        public int Picker { get; set; }
        private (List<PartnerHistoryModel> partners, int count) _partnerHistoryModel_Data = (new List<PartnerHistoryModel>(), 0);
        private (List<ContactHistoryModel> contacts, int count) _contactHistoryModel_Data = (new List<ContactHistoryModel>(), 0);

        private readonly PartnerApiPagination _partnerApiPagination = new();
        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            LoadDataArgs args = new LoadDataArgs()
            {
                Filter = "",
                Skip = 0,
                OrderBy = "",
                Top = _partnerApiPagination.PageSize,
            };

            await LoadData(args);
        }
        private async Task LoadData(LoadDataArgs args)
        {
            try
            {
                var skip = args.Skip ?? 0;
                var page = (int)(skip / _partnerApiPagination.PageSize) + 1;
                var filter = args.Filter;
                if (Picker == 0)
                {
                    _partnerHistoryModel_Data = await PartnerRepository.Get_All_Partner_History_From_Specific_Partner(Partner.PartnerGUID, page, _partnerApiPagination.PageSize);
                    Count = _partnerHistoryModel_Data.count;
                    IsLoading = false;
                }
                else
                {
                    _contactHistoryModel_Data = await PartnerRepository.Get_All_Contact_History_From_Specific_Partner_And_Contact(Partner.PartnerGUID, Contact.ContactGUID, page, _partnerApiPagination.PageSize);
                    Count = _contactHistoryModel_Data.count;
                    IsLoading = false;
                }
            }
            catch (ApiException apiex)
            {
                await DialogService.Alert($"{Localization["TryAgain"]}", $"{Localization["Alert"]}");
            }
        }
        void RenderRows(RowRenderEventArgs<PartnerHistoryModel> args)
        {
            switch (args.Data.Action)
            {
                case "Create":
                    args.Attributes.Add("style", "background-color: rgba(197, 255, 189, 0.8);");
                    break;
                case "Delete":
                    args.Attributes.Add("style", "background-color: rgba(255, 168, 168, 0.8);");
                    break;
                case "Update":
                    args.Attributes.Add("style", "background-color: rgba(252, 252, 239, 0.8);");
                    break;
            }
        }
        void RenderRows(RowRenderEventArgs<ContactHistoryModel> args)
        {
            switch (args.Data.Action)
            {
                case "Create":
                    args.Attributes.Add("style", "background-color: rgba(197, 255, 189, 0.8);");
                    break;
                case "Delete":
                    args.Attributes.Add("style", "background-color: rgba(255, 168, 168, 0.8);");
                    break;
                case "Update":
                    args.Attributes.Add("style", "background-color: rgba(252, 252, 239, 0.8);");
                    break;
            }
        }
    }
}
