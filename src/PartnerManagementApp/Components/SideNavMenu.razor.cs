using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Radzen;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.Components
{
    [Authorize(Policy = "Supervisor")]
    public partial class SideNavMenu
    {
        [Inject]
        private GraphServiceClient GraphClient { get; set; } = null!;
        [Inject]
        private MicrosoftIdentityConsentAndConditionalAccessHandler ConsentHandler { get; set; } = null!;

        protected User _user = new User();
        protected override async Task OnInitializedAsync()
        {
            await GetUserProfile();
        }

        private async Task GetUserProfile()
        {
            try
            {
                var request = GraphClient?.Me.Request();
                if (request != null)
                {
                    _user = await request.GetAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ConsentHandler?.HandleException(ex);
            }
        }
    }
}
