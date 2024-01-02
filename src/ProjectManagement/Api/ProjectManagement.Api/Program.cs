using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.Json.Serialization;

using MainHub.Internal.PeopleAndCulture.ProjectManagement.API.HealthCheck;
using Serilog;
using Microsoft.FeatureManagement;
using Microsoft.EntityFrameworkCore;
using MainHub.Internal.PeopleAndCulture.ProjectManagement.Data;
using MainHub.Internal.PeopleAndCulture;
using MainHub.Internal.PeopleAndCulture.ProjectManagement.Repository;

var builder = WebApplication.CreateBuilder();

// Add AzureAD
builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Add Serilog
var logger = new Serilog.LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog(logger);


// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

// Feature Management
builder.Services.AddFeatureManagement();

// Health Check
builder.Services.AddHealthChecks()
    .AddCheck<ProjectManagementHealthCheck>(nameof(ProjectManagementHealthCheck));

// Inject DBContext  and Repository
builder.Services.AddDbContext<ProjectManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SkillHubDb")));

builder.Services.AddScoped<IProjectManagementRepository, ProjectManagementRepository>();

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
// Use Serilog
app.UseSerilogRequestLogging();

// Global Exception Handler
app.UseExceptionHandler("/error");

// Map Health Checks
app.MapHealthChecks("/api/projects/status");


app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
