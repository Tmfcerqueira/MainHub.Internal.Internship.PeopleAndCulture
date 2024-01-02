using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using MainHub.Internal.PeopleAndCulture.ProjectManagement.API.Feature;
using MainHub.Internal.PeopleAndCulture.ProjectManagement.API.Extensions;
using MainHub.Internal.PeopleAndCulture.ProjectManagement.API.Models;

namespace MainHub.Internal.PeopleAndCulture.ProjectManagement.API.Controllers
{

    [FeatureGate(ProjectManagementFeatureFlags.ProjectManagement)]
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IFeatureManager _featureManager;
        private readonly IProjectManagementRepository _repository;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ILogger<ProjectController> logger, IFeatureManager featureManager, IProjectManagementRepository repository)
        {
            _logger = logger;
            _featureManager = featureManager;
            _repository = repository;
        }

        /// <summary>
        /// Get a lists of Projects.
        /// </summary>
        /// <returns>A lists of Projects.</returns>
        [FeatureGate(ProjectManagementFeatureFlags.ProjectManagement_Index)]
        [HttpGet()]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Project>>> Index()
        {
            return Ok((await _repository.GetProjectsAsync()).ToProjectsApiModel());
        }

    }
}
