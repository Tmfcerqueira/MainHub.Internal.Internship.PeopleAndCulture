using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.IdentityModel.Tokens.Jwt;
using PartnerManagement.App.Repository;
using PartnerManagement.Api.Proxy.Client.Api;
using Radzen;
using Microsoft.AspNetCore.Components;
using MainHub.Internal.PeopleAndCulture;
using PartnerManagement.Api.Proxy.Client.Client;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.Resources;

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

builder.Services.AddScoped<IPartnerApi, PartnerApi>(
    c =>
    {
        Configuration apiConfig = GetConfiguration(c, builder);
        return new PartnerApi(apiConfig);
    });

builder.Services.AddScoped(
 c =>
 {
     var partnerApi = c.GetRequiredService<IPartnerApi>();
     return new PartnerAppRepository(partnerApi);
 });
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("Supervisor", policy =>
        policy.RequireRole("SkillHub.Supervisor"));
});

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<TooltipService>();

builder.Services.AddLocalization();
builder.Services.AddScoped<IStringLocalizer<Resource>, StringLocalizer<Resource>>();

builder.Services.AddRazorPages();
builder.Services
.AddServerSideBlazor()
.AddMicrosoftIdentityConsentHandler();
static Configuration GetConfiguration(IServiceProvider c, WebApplicationBuilder builder)
{
    string accessToken = AuthorizationOperations.GetToken(c, builder);

    var apiConfig = new Configuration
    {
        BasePath = builder.Configuration.GetSection("ProxyClient")["PartnerApiEndpoint"], // Alterar o Endpoint

        AccessToken = accessToken
    };
    return apiConfig;
}
//Redirect page to the main one (index) após logout!
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

var serviceProvider = app.Services;

app.Run();
