using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Shared.HeaderAreaComponent
{
    public partial class SwitchThemeArea
    {
        private string _theme = "dark"; // default theme is dark

        [Inject]
        private NavigationManager Navigation { get; set; } = null!;

        [Inject]
        private ProtectedLocalStorage ProtectedLocalStorage { get; set; } = null!;


        private async Task SetTheme(string theme)
        {
            _theme = theme;

            await ProtectedLocalStorage.SetAsync("theme", _theme);

            Navigation.NavigateTo(Navigation.Uri, true);
        }
    }
}
