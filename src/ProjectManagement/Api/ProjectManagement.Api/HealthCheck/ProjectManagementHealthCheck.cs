using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MainHub.Internal.PeopleAndCulture.ProjectManagement.API.HealthCheck
{
    public class ProjectManagementHealthCheck : IHealthCheck

    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var isHealthy = true;

            // TODO: HEALTHINESS CHECK

            if (isHealthy)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("A healthy result."));
            }

            return Task.FromResult(
                new HealthCheckResult(
                    context.Registration.FailureStatus, "An unhealthy result."));

            throw new NotImplementedException();
        }
    }
}
