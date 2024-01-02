using Microsoft.AspNetCore.Components;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.Components;
using Microsoft.AspNetCore.Authorization;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Shared.Private
{
    [Authorize(Policy = "Supervisor")]
    public partial class PrivateLayout
    {
        public PrivateLayout(NavigationManager navigationManager)
        {
            NavigationManager = navigationManager;
        }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected void OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                NavigationManager?.NavigateTo("/dashboard");
            }
        }
    }
}
