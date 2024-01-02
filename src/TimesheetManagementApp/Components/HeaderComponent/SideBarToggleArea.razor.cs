using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.Components.HeaderComponent
{
    public partial class SideBarToggleArea
    {
        [Inject]
        private IJSRuntime JsRuntime { get; set; } = null!;

        static async Task ToggleSideBar(IJSRuntime jsRuntime)
        {
            await jsRuntime.InvokeVoidAsync("toggleSideBar");
        }
    }
}
