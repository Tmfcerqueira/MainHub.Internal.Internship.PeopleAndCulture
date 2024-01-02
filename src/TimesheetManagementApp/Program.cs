using System.IdentityModel.Tokens.Jwt;
using MainHub.Internal.PeopleAndCulture;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Radzen;
using TimesheetManagement.Api.Proxy.Client.Api;
using TimesheetManagement.Api.Proxy.Client.Client;
using ProjectManagement.Api.Proxy.Client;
using ProjectManagement.Api.Proxy.Client.Api;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
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

builder.Services.AddScoped<ITimesheetApi, TimesheetApi>(
    c =>
    {
        Configuration apiConfig = GetConfiguration(c, builder);
        return new TimesheetApi(apiConfig);
    });

builder.Services.AddScoped<ITimesheetActivityApi, TimesheetActivityApi>(
    c =>
    {
        Configuration apiConfig = GetConfiguration(c, builder);
        return new TimesheetActivityApi(apiConfig);
    });

builder.Services.AddScoped<IAdminApi, AdminApi>(
    c =>
    {
        Configuration apiConfig = GetConfiguration(c, builder);
        return new AdminApi(apiConfig);
    });

builder.Services.AddScoped<IProjectApi, ProjectApi>(
    c =>
    {
        ProjectManagement.Api.Proxy.Client.Client.Configuration apiConfig = GetProjectsConfiguration(c, builder);
        return new ProjectApi(apiConfig);
    });

builder.Services.AddScoped<IProjectActivityApi, ProjectActivityApi>(
    c =>
    {
        ProjectManagement.Api.Proxy.Client.Client.Configuration apiConfig = GetProjectsConfiguration(c, builder);
        return new ProjectActivityApi(apiConfig);
    });

builder.Services.AddScoped<ITimesheetAppRepository, TimesheetAppRepository>();

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
        BasePath = builder.Configuration.GetSection("ProxyClient")["TimesheetApiEndpoint"],

        AccessToken = accessToken
    };
    return apiConfig;
}

static ProjectManagement.Api.Proxy.Client.Client.Configuration GetProjectsConfiguration(IServiceProvider c, WebApplicationBuilder builder)
{
    string accessToken = AuthorizationOperations.GetToken(c, builder);

    var apiConfig = new ProjectManagement.Api.Proxy.Client.Client.Configuration

    {
        BasePath = builder.Configuration.GetSection("ProxyClient")["ProjectApiEndpoint"],

        AccessToken = accessToken
    };
    return apiConfig;
}

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
