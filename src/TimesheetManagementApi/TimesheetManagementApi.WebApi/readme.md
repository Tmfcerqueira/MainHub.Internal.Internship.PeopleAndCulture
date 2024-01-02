# Feature Flag Setup

```csharp
    // Feature Management
    builder.Services.AddFeatureManagement();
```

# Health Check Setup

Need to develop the healthcheck logic.

```csharp
    // Health Check
    builder.Services.AddHealthChecks()
        .AddCheck<TimesheetManagementApiHealthCheck>("TimesheetManagementApi");
```

```csharp
    // Map Health Checks
    app.MapHealthChecks("status");
```

# Open Telemetry

# Resources:

- [Health checks in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks)
- [Quickstart: Add feature flags to an ASP.NET Core app](https://learn.microsoft.com/en-us/azure/azure-app-configuration/quickstart-feature-flag-aspnet-core)
- [Configuration in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
