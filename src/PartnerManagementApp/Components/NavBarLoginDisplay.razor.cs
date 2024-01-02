using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Http;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.Components
{
    [Authorize(Policy = "Supervisor")]
    public partial class NavBarLoginDisplay
    {
        [Inject]
        private GraphServiceClient GraphClient { get; set; } = null!;
        [Inject]
        private MicrosoftIdentityConsentAndConditionalAccessHandler ConsentHandler { get; set; } = null!;
        protected string? _photo;
        protected User _user = new User();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var request = GraphClient?.Me.Request();
                if (request != null)
                {
                    _user = await request.GetAsync();
                }
                _photo = await GetPhoto();
            }
            catch (Exception ex)
            {
                ConsentHandler?.HandleException(ex);
            }
        }
        protected async Task<string> GetPhoto()
        {
            string photo = string.Empty;
            try
            {
                if (GraphClient != null)
                {
                    using (var photoStream = await GraphClient.Me.Photo.Content.Request().GetAsync())
                    {
                        byte[] photoByte = ((System.IO.MemoryStream)photoStream).ToArray();
                        photo = Convert.ToBase64String(photoByte);
                        this.StateHasChanged();
                    }
                }
            }
            catch (Exception)
            {
                photo = string.Empty;
            }
            return photo;
        }
    }
}
