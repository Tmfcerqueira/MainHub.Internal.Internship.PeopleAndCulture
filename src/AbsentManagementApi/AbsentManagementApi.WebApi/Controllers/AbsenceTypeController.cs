using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Extensions;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Controllers
{
    [Authorize(Roles = "SkillHub.User,SkillHub.Supervisor")]
    [Route("api/[controller]")]
    [ApiController]
    public class AbsenceTypeController : ControllerBase
    {
        public readonly IAbsenceRepository AbsenceRepository;

        public AbsenceTypeController(IAbsenceRepository absenceRepository)
        {
            this.AbsenceRepository = absenceRepository;
        }

        /// <summary>
        /// Creates an AbsenceType.
        /// </summary>
        ///<param name = "model" > The AbsenceTypeCreateRequestModel object used to create an AbsenceType.</param>
        /// <param name="actionBy">The person who created</param>
        /// <returns>AbsenceTypeCreateResponseModel</returns>
        /// /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AbsenceTypeCreateResponseModel>> CreateAbsenceType(AbsenceTypeCreateRequestModel model, Guid actionBy)
        {
            var repoModel = model.ToAbsenceTypeRepoModel();

            //fill guid
            repoModel.TypeGuid = Guid.Empty;

            // Save the absenceType entity to the database
            var createResponse = await AbsenceRepository.CreateAbsenceType(repoModel, actionBy);

            // Create the response model from the saved absenceType entity
            var response = createResponse.ToAbsenceTypeCreateResponseModel();

            // Return the response model with HTTP status 200 OK
            return Ok(response);
        }


        /// <summary>
        /// Get all absence types.
        /// </summary>
        /// <returns>List of AbsenceTypeRepoModel objects</returns>
        /// <response code="200">Returns the list of all AbsenceTypeRequestModel objects</response>
        [HttpGet("types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AbsenceTypeResponseModel>>> GetAllAbsenceTypes()
        {
            var absenceTypes = await AbsenceRepository.GetAllAbsenceTypes();
            var responseModels = absenceTypes.Select(a => a.ToAbsenceTypeResponseModel()).ToList();
            return Ok(responseModels);
        }

    }
}
