using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.IdentityModel.Tokens.Jwt;
using PeopleManagement.Api.Proxy.Client.Api;
using App.Repository;
using Radzen;
using MainHub.Internal.PeopleAndCulture;
using PeopleManagement.Api.Proxy.Client.Client;
using Microsoft.Graph;
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using MainHub.Internal.PeopleAndCulture.PeopleManagement.Resources;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
var initialScopes = builder.Configuration.GetValue<string>("GraphAPI:Scopes")?.Split(' ');
JwtSecurityTokenHandler.DefaultMapInboundClaims = true;



builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
    .AddMicrosoftGraph(builder.Configuration.GetSection("GraphAPI"))
    .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews()
 .AddMicrosoftIdentityUI();

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("Supervisor", policy =>
        policy.RequireRole("SkillHub.Supervisor"));
});

builder.Services.AddLocalization();
builder.Services.AddScoped<IStringLocalizer<Resource>, StringLocalizer<Resource>>();


static Configuration GetConfiguration(IServiceProvider c, WebApplicationBuilder builder)

{

    string accessToken = AuthorizationOperations.GetToken(c, builder);

    var apiConfig = new Configuration
    {
        BasePath = builder.Configuration.GetSection("ProxyClient")["PeopleApiEndpoint"],
        AccessToken = accessToken
    };

    return apiConfig;

}
builder.Services.Configure<AuthorizationOptions>(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddRequirements(new DenyAnonymousAuthorizationRequirement())
        .RequireAssertion(context =>
        {
            // Check if the user has the "Supervisor" role claim
            if (!context.User.HasClaim(c => c.Type == "SkillHub.Supervisor"))
            {
                // Get the HttpContext from the AuthorizationHandlerContext
                var httpContext = (context.Resource as HttpContextAccessor)?.HttpContext;
                if (httpContext != null)
                {
                    // Redirect the user to a different page
                    httpContext.Response.Redirect("/AccessDenied");
                    return false;
                }
            }

            return true;
        })
        .Build();
});



builder.Services.AddScoped<ICollaboratorApi, CollaboratorApi>(
    c =>
    {
        Configuration apiConfig = GetConfiguration(c, builder);
        return new CollaboratorApi(apiConfig);
    });

builder.Services.AddScoped(
    c =>
    {
        var peopleApi = c.GetRequiredService<ICollaboratorApi>();
        var serviceClient = c.GetRequiredService<GraphServiceClient>();
        return new PeopleAppRepository((CollaboratorApi)peopleApi, serviceClient);
    });

builder.Services.AddRazorPages();
builder.Services
 .AddServerSideBlazor()
 .AddMicrosoftIdentityConsentHandler();

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<TooltipService>();




builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Events.OnSignedOutCallbackRedirect = context =>
    {
        // Set the post-logout redirect URI to your index page
        context.Response.Redirect("/");



        // Call the default event handler to continue the logout process
        context.HandleResponse();



        return Task.CompletedTask;
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

var supportedCultures = new[] { "en-US", "pt-PT" };

var localizationOptions = new RequestLocalizationOptions()

    .SetDefaultCulture(supportedCultures[0])

    .AddSupportedCultures(supportedCultures)

    .AddSupportedUICultures(supportedCultures);


app.UseRequestLocalization(localizationOptions);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();
