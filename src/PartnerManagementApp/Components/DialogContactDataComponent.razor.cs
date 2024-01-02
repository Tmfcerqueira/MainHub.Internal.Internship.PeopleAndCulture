using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PartnerManagement.App.Models;
using PartnerManagement.App.Repository;
using Radzen;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.Components
{
    [Authorize(Policy = "Supervisor")]
    public partial class DialogContactDataComponent
    {
        [Inject]
        private DialogService DialogService { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        private PartnerAppRepository PartnerRepository { get; set; } = null!;

        [Parameter]
        public DataGridRowMouseEventArgs<ContactModel> ContactArgs { get; set; } = null!;
        [Parameter]
        public int FormPicker { get; set; }


        protected ContactModel _model = new();
        private readonly PartnerApiPagination _partnerApiPagination = new();
        private void Cancel()
        {
            DialogService.Close();
        }
    }
}
