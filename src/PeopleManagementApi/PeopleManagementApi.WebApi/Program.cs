global using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using PeopleManagementRepository.Models;
using PeopleManagementRepository;
using PeopleManagementRepository.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Graph;
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

var initialScopes = builder.Configuration.GetValue<string>("GraphAPI:Scopes")?.Split(' ');
JwtSecurityTokenHandler.DefaultMapInboundClaims = true;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddMicrosoftGraph(builder.Configuration.GetSection("GraphAPI"))
    .AddInMemoryTokenCaches();

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("Supervisor", policy =>
        policy.RequireRole("SkillHub.Supervisor"));
});

builder.Services.AddHealthChecks()
    .AddCheck("api", () => HealthCheckResult.Healthy("API is healthy!"));

builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();
builder.Services.AddDbContext<PeopleManagementDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SkillHubDb")));
builder.Services.AddScoped<GraphServiceClient>();






// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Swagger Doc
    c.SwaggerDoc(
    "v1",
    new OpenApiInfo
    {
        Title = "MainHub.Internal.Internship.PeopleAndCulture.PeopleManagementApi.WebApi",
        Version = "v1"
    });

    // Swagger Security Definition
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "OAuth2.0 Auth Code with PKCE",
        Name = "oauth2",
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl =
            new Uri(builder.Configuration["AzureAdAPI:AuthorizationUrl"]),
                TokenUrl =
            new Uri(builder.Configuration["AzureAdAPI:TokenUrl"]),
                Scopes = new Dictionary<string, string>
            {
                {
                    builder.Configuration["AzureAdAPI:ApiScope"],
                    "read the api"
                }
            }
            }
        }
    });

    // Swagger Security Requirement
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { builder.Configuration["AzureAdAPI:ApiScope"] }
        }
    });


});

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        c =>
        {
            c.SwaggerEndpoint(
                "v1/swagger.json",
                "MainHub.Internal.Internship.PeopleAndCulture.PeopleManagementApi.WebApi v1");
            c.OAuthClientId(
                builder.Configuration["AzureAdAPI:OpenIdClientId"]
                );
            c.OAuthUsePkce();
            c.OAuthScopeSeparator(" ");
        }
    );

}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
