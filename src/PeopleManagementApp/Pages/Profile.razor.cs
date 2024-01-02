using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace MainHub.Internal.PeopleAndCulture.PeopleManagement.Pages
{
    public partial class Profile
    {
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Inject]
        MicrosoftIdentityConsentAndConditionalAccessHandler ConsentHandler { get; set; } = null!;




        User _user = new User();
        string _photo = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _user = await GraphServiceClient.Me.Request().GetAsync();
                var r = await GraphServiceClient.Me.AppRoleAssignments.Request().GetAsync();
                _photo = await GetPhoto();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ConsentHandler.HandleException(ex);
            }
        }

        protected async Task<string> GetPhoto()
        {
            string photo;

            try
            {
                using (var photoStream = await GraphServiceClient.Me.Photo.Content.Request().GetAsync())
                {
                    byte[] photoByte = ((System.IO.MemoryStream)photoStream).ToArray();
                    photo = Convert.ToBase64String(photoByte);
                    this.StateHasChanged();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                photo = string.Empty;
            }
            return photo;
        }
    }
}
