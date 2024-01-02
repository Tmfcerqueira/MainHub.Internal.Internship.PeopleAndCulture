using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using PeopleManagementRepository.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));


//HealthCheck
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("SkillHubDb"));

builder.Services.AddHealthChecksUI().AddInMemoryStorage();

// Add services to the container.
builder.Services.AddControllers();

// Inject AbsenceManagementDbContext and Repository
builder.Services.AddDbContext<AbsenceManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SkillHubDb")));

builder.Services.AddScoped<IAbsenceRepository, AbsenceRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    // Swagger Doc
    c.SwaggerDoc(
    "v1",
    new OpenApiInfo
    {
        Title = "MainHub.Internal.Internship.PeopleAndCulture.API",
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

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        c =>
        {
            c.SwaggerEndpoint(
                "v1/swagger.json",
                "MainHub.Internal.Internship.PeopleAndCulture.API v1");
            c.OAuthClientId(
                builder.Configuration["AzureAdAPI:OpenIdClientId"]
                );
            c.OAuthUsePkce();
            c.OAuthScopeSeparator(" ");
        }
    );

}

app.UseHttpsRedirection();

app.MapHealthChecks("/api/absence/healthcheck");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
