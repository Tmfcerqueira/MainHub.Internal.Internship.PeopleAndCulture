using System.IdentityModel.Tokens.Jwt;
using AbsentManagement.Api.Proxy.Client.Api;
using AbsentManagement.Api.Proxy.Client.Client;
using App.Repository;
using MainHub.Internal.PeopleAndCulture;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using PeopleManagement.Api.Proxy.Client.Api;
using Radzen;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var initialScopes = builder.Configuration.GetValue<string>("GraphAPI:Scopes")?.Split(' ');
JwtSecurityTokenHandler.DefaultMapInboundClaims = true;

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
        .AddMicrosoftGraph(builder.Configuration.GetSection("GraphAPI"))
        .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews()
     .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SkillHub.User", policy =>
        policy.RequireRole("SkillHub.User"));

    options.AddPolicy("SkillHub.Supervisor", policy =>
        policy.RequireRole("SkillHub.Supervisor"));
});

builder.Services.AddLocalization();


builder.Services.AddScoped<IAbsenceApi, AbsenceApi>(
    c =>
    {
        Configuration apiConfig = GetConfiguration(c, builder);
        return new AbsenceApi(apiConfig);
    });

builder.Services.AddScoped<IAbsenceTypeApi, AbsenceTypeApi>(
    c =>
    {
        Configuration apiConfig = GetConfiguration(c, builder);
        return new AbsenceTypeApi(apiConfig);
    });

builder.Services.AddScoped<IAdminApi, AdminApi>(
    c =>
    {
        Configuration apiConfig = GetConfiguration(c, builder);
        return new AdminApi(apiConfig);
    });

builder.Services.AddScoped<ICollaboratorApi, CollaboratorApi>(
    c =>
    {
        PeopleManagement.Api.Proxy.Client.Client.Configuration apiConfig = GetPeopleConfiguration(c, builder);
        return new CollaboratorApi(apiConfig);
    });


builder.Services.AddScoped<IAbsenceAppRepository, AbsenceAppRepository>();

builder.Services.AddScoped<DialogService>();

builder.Services.AddScoped<TooltipService>();

builder.Services.AddRazorPages();
builder.Services
    .AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();

static Configuration GetConfiguration(IServiceProvider c, WebApplicationBuilder builder)

{

    string accessToken = AuthorizationOperations.GetToken(c, builder);

    var apiConfig = new Configuration

    {

        BasePath = builder.Configuration.GetSection("ProxyClient")["AbsenceApiEndpoint"],

        AccessToken = accessToken

    };

    return apiConfig;

}

static PeopleManagement.Api.Proxy.Client.Client.Configuration GetPeopleConfiguration(IServiceProvider c, WebApplicationBuilder builder)

{

    string accessToken = AuthorizationOperations.GetToken(c, builder);

    var apiConfig = new PeopleManagement.Api.Proxy.Client.Client.Configuration
    {
        BasePath = builder.Configuration.GetSection("ProxyClient")["PeopleApiEndpoint"],
        AccessToken = accessToken
    };

    return apiConfig;

}


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

var supportedCultures = new[] { "en-US", "pt-PT" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
