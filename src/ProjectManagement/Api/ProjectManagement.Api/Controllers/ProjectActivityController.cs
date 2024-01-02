using System.Collections;
using MainHub.Internal.PeopleAndCulture.ProjectManagement.API.Extensions;
using MainHub.Internal.PeopleAndCulture.ProjectManagement.API.Feature;
using MainHub.Internal.PeopleAndCulture.ProjectManagement.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace MainHub.Internal.PeopleAndCulture.ProjectManagement.API.Controllers
{
    [FeatureGate(ProjectManagementFeatureFlags.ProjectActivityManagement)]
    [ApiController]
    [Route("[controller]")]
    public class ProjectActivityController : Controller
    {
        private readonly IFeatureManager _featureManager;
        private readonly ILogger<ProjectController> _logger;
        private readonly IProjectManagementRepository _repository;

        public ProjectActivityController(IFeatureManager featureManager, ILogger<ProjectController> logger, IProjectManagementRepository repository)
        {
            _featureManager = featureManager;
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Gets a list of project activities.
        /// </summary>
        /// <returns>List of project activities.</returns>
        [FeatureGate(ProjectManagementFeatureFlags.ProjectActivityManagement_Index)]
        [HttpGet()]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProjectActivity>>> GetAllProjectActivities()
        {
            await Task.CompletedTask;
            return Ok((await _repository.GetProjectActivities()).ToProjectActivitiesApiModel());
        }
    }
}
