using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Graph;
using Radzen;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Shared.HeaderAreaComponent
{
    [Authorize]
    public partial class ProfileArea
    {
        [Inject]
        GraphServiceClient GraphServiceClient { get; set; } = null!;

        [Inject]
        private ProtectedSessionStorage ProtectedSessionStorage { get; set; } = null!;


        User _user = new();
        string _photo = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {

                _user = await GraphServiceClient.Me.Request().GetAsync();
                _photo = await GetPhoto();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<string> GetPhoto()
        {
            var photo = string.Empty;

            try
            {
                using var photoStream = await GraphServiceClient.Me.Photo.Content.Request().GetAsync();
                var photoByte = ((MemoryStream)photoStream).ToArray();
                photo = Convert.ToBase64String(photoByte);
                this.StateHasChanged();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                photo = string.Empty;
            }

            return photo;
        }

        async Task SignOut()
        {
            await ProtectedSessionStorage.DeleteAsync("logged");
        }

    }
}
