using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.Shared.Private
{
    public partial class PrivateMainLayout
    {

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;


        [Inject]
        private ProtectedSessionStorage ProtectedSessionStorage { get; set; } = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            bool logged = false;

            var storageValue = await ProtectedSessionStorage.GetAsync<bool>("logged");

            if (storageValue.Value)
            {
                var setLogged = await ProtectedSessionStorage.GetAsync<bool>("logged");
                logged = setLogged.Value;
            }


            if (firstRender && logged == false)
            {
                NavigationManager.NavigateTo("/dashboard");
                await ProtectedSessionStorage.SetAsync("logged", true);
            }
        }
    }
}
