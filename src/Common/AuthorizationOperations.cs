using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;

namespace MainHub.Internal.PeopleAndCulture
{
    public static class AuthorizationOperations
    {
        public static string GetToken(IServiceProvider c, WebApplicationBuilder builder)
        {
            var scopes = builder.Configuration.GetValue<string>("AzureAd:Scopes")?.Split(' ') ?? new string[1];
            var tokenAcquisition = c.GetRequiredService<ITokenAcquisition>();
            var accessToken = tokenAcquisition.GetAccessTokenForUserAsync(scopes).Result;
            return accessToken;
        }

    }
}
