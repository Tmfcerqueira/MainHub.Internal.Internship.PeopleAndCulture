using Microsoft.AspNetCore.Components;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Shared.HeaderAreaComponent;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Radzen;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Shared.Private
{
    public partial class PrivateMainLayout
    {
        private string _theme = "dark"; // default theme is dark


        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;


        [Inject]
        private ProtectedLocalStorage ProtectedLocalStorage { get; set; } = null!;

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


            var themeResult = await ProtectedLocalStorage.GetAsync<string>("theme");
            var theme = themeResult.Value;

            if (theme != _theme)
            {
                Console.WriteLine("Theme switched:" + theme);
                _theme = theme!;
                await InvokeAsync(StateHasChanged); // Only call StateHasChanged when _theme has been updated
            }
        }

        public string GetSidenavClass()
        {
            if (string.IsNullOrEmpty(_theme))
            {
                return "sb-sidenav-dark";
            }
            if (_theme == "dark")
            {
                return "sb-sidenav-dark";
            }
            else
            {
                return "sb-sidenav-light";
            }
        }
    }
}
