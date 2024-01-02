using Microsoft.AspNetCore.Mvc;
using PeopleManagementRepository.Models;
using MainHub.Internal.PeopleAndCulture.PeopleManagement.API.Extensions;
using PeopleManagement.Api.Models;
using MainHub.Internal.PeopleAndCulture.Extensions;
using PeopleManagementRepository.Extensions;
using Microsoft.Graph;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MainHub.Internal.PeopleAndCulture.PeopleManagement.API.Controllers
{
    [Authorize(Policy = "Supervisor")]
    [Route("api/people")]
    [Produces("application/json")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly IPeopleRepository _peopleRepository;
        private const int DEFAULT_PAGE_SIZE = 40;
        private const int DEFAULT_MIN_PAGE_SIZE = 1;
        public CollaboratorController(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;
        }

        /// <summary>
        /// Creates a Person.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A newly created Person</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /CreateCollaborator
        ///     {
        ///         "peopleGUID": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "name": "string",
        ///         "birthDate": "2023-04-28T14:00:02.923Z",
        ///         "adress": "string",
        ///         "postal": "string",
        ///         "country": "string",
        ///         "taxNumber": "string",
        ///         "ccNumber": "string",
        ///         "ssNumber": "string",
        ///         "ccVal": "2023-04-28T14:00:02.923Z",
        ///         "civilState": "string",
        ///         "dependentNum": 0,
        ///         "entryDate": "2023-04-28T14:00:02.923Z",
        ///         "exitDate": "2023-04-28T14:00:02.923Z",
        ///         "creationDate": "2023-04-28T14:00:02.923Z",
        ///         "createdBy": "string",
        ///         "changeDate": "2023-04-28T14:00:02.923Z",
        ///         "changedBy": "string",
        ///         "status": "string"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the newly created Person</response>
        /// <response code="400">If the item is null</response>
        [HttpPost(Name = "CreateCollaboratorAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<ApiCollaboratorCreateResponseModel>> CreateCollaboratorAsync(ApiCollaboratorCreateRequestModel model)
        {
            try
            {
                var claim = User.Claims.First(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                var userId = Guid.Parse(claim);

                var repoModel = model.ToPeopleRepoModel();
                // Save the Person entity to the database
                var createResponse = await _peopleRepository.CreateCollaboratorAsync(repoModel, userId);

                // Create the response model from the saved Person entity
                var response = createResponse.ToPeopleCreateResponseModel();

                // Return the response model with HTTP status 200 OK
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get all collaborators
        /// </summary>
        /// <returns>List of Collaborators with limited properties</returns>
        /// <response code="200">Returns the list of all the collaborators</response>
        /// <response code="404">If there are no collaborators</response>
        [HttpGet(Name = "GetAllCollaborators")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiCollaboratorResponseWithCount>> GetCollaborators(int page, int pageSize, string? filter, State? state)
        {
            try
            {
                if (page <= 0)
                {
                    page = 1;
                }
                if (pageSize < DEFAULT_MIN_PAGE_SIZE || pageSize > DEFAULT_PAGE_SIZE)
                {
                    pageSize = DEFAULT_PAGE_SIZE;
                }
                if (state == null || !Enum.IsDefined(typeof(State), state))
                {
                    state = State.Active;
                }

                var collaborator = await _peopleRepository.GetAllCollaborators(page, pageSize, filter!, state);
                var result = collaborator.Select(c => c.ToAllApiResponseModel()).ToList();
                var totalCount = _peopleRepository.GetAllCollaborators(1, 500, filter!, state).Result.Count;
                var collaboratorPaged = new ApiCollaboratorResponseWithCount()
                {
                    Collaborators = result,
                    TotalCount = totalCount,
                };
                return Ok(collaboratorPaged);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get one collaborator
        /// </summary>
        /// <returns>A specific collaborator with all of its properties</returns>
        /// <response code="200">Returns one collaborator that matches the guid id given</response>
        /// <response code="404">If there are no collaborators with that guid id</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiCollaboratorResponseModel>> GetOneCollaborator(Guid id)
        {
            try
            {
                var collaborator = await _peopleRepository.GetOneCollaborator(id);
                var result = collaborator!.ToApiResponseModel();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Delete a collaborator
        /// </summary>
        /// <returns>Deletes a specific collaborator</returns>
        /// <response code="204">Returns no content if the delete is successfull</response>
        /// <response code="404">Failed to delete the collaborator</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{personId}")]
        public async Task<IActionResult> DeleteCollaborator(Guid personId)
        {
            var claim = User.Claims.First(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var userId = Guid.Parse(claim);
            var deleted = await _peopleRepository.DeleteCollaboratorAsync(personId, userId);

            if (deleted)
            {
                return NoContent(); // Successful deletion, return 204 No Content
            }

            return NotFound(); // Collaborator not found, return 404 Not Found
        }

        /// <summary>
        /// Update a collaborator
        /// </summary>
        /// <returns>Deletes a specific collaborator</returns>
        /// <response code="204">Returns no content if the update is successfull</response>
        /// <response code="404">If there are no collaborators with that guid id</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{personId}")]
        public async Task<ActionResult> UpdateCollaborator(Guid personId, [FromBody] ApiCollaboratorUpdateModel collaborator)
        {
            var claim = User.Claims.First(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var userId = Guid.Parse(claim);
            var existingCollaborator = await _peopleRepository.GetOneCollaborator(personId);

            if (existingCollaborator == null)
            {
                return NotFound(); // Collaborator not found
            }

            var updatedCollaborator = collaborator.ToPeopleRepoModel();


            await _peopleRepository.UpdateCollaboratorAsync(personId, updatedCollaborator, userId);


            return NoContent();
        }

        /// <summary>
        /// Get a collaborator history
        /// </summary>
        /// <returns>Deletes a specific collaborator</returns>
        /// <response code="200">Returns all the history from that collaborator</response>
        /// <response code="404">If there are no history for that collaborator</response>
        [HttpGet("{id}/history")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ApiCollaboratorHistoryResponseModel>>> GetCollaboratorHistory(Guid id, int page, int pageSize)
        {
            if (page <= 0)
            {
                page = 1;
            }

            if (pageSize < DEFAULT_MIN_PAGE_SIZE || pageSize > DEFAULT_PAGE_SIZE)
            {
                pageSize = DEFAULT_PAGE_SIZE;
            }

            var collaboratorHistory = await _peopleRepository.GetCollaboratorHistory(id, page, pageSize);
            var result = collaboratorHistory.Select(h => h.ToHistoryResponseModel()).ToList();

            if (result == null || result.Count == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
