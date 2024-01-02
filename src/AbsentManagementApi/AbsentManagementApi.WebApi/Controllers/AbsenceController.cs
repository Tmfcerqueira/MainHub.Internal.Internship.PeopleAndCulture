using MainHub.Internal.PeopleAndCulture.AbsentManagement;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Extensions;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Extensions;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;
using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Newtonsoft.Json;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Controllers
{
    [Authorize(Roles = "SkillHub.User")]
    [ApiController]
    [Route("api/people/{personGuid}/absence")]
    public class AbsenceController : Controller
    {
        private const int DEFAULT_START_PAGE = 1;


        public readonly IAbsenceRepository AbsenceRepository;

        public AbsenceController(IAbsenceRepository absenceRepository)
        {
            this.AbsenceRepository = absenceRepository;
        }

        //creates 

        /// <summary>
        /// Creates an Absence.
        /// </summary>
        /// <param name="personGuid">The person for who is the absence created</param>
        ///<param name = "model" > The AbsenceCreateRequestModel object used to create an Absence.</param>
        /// <param name="actionBy">Who created</param>
        /// <returns></returns>
        /// /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost()]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<AbsenceCreateResponseModel>> CreateAbsence(Guid personGuid, AbsenceCreateRequestModel model, Guid actionBy)
        {

            if (personGuid != model.PersonGuid)
            {
                return BadRequest("Person is not equal");
            }

            //convert request to repo
            var requestModel = model.ToAbsenceRepoModel();

            // Save the absence entity to the database
            var createResponse = await AbsenceRepository.CreateAbsence(requestModel, actionBy);

            // convert to response
            var response = createResponse.ToAbsenceCreateResponseModel();


            // Return the response model with HTTP status 200 OK
            return Ok(response);
        }


        //gets


        /// <summary>
        /// Get all absences for a specific person and year.
        /// </summary>
        /// <param name="personGuid">The person ID</param>
        /// <param name="year">The year to filter by</param>
        /// <param name="page">The page</param>
        /// <param name="pageSize">The amount of data per page</param>
        /// <param name="status"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>List of AbsenceResponseModel objects</returns>
        /// <response code="200">Returns the AbsenceResponseModels for the person and year</response>
        /// <response code="404">If no absences are found for the person and year</response>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AbsenceResponseModels>> GetAbsencesByPerson(Guid personGuid, int year, int page, int pageSize, ApprovalStatus status, DateTime startDate, DateTime endDate)
        {
            if (page < DEFAULT_START_PAGE)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > 20)
            {
                pageSize = 20;
            }

            // Get the absences for the person and year, but only retrieve the page of items
            var absencesResult = await AbsenceRepository.GetAbsencesByPerson(personGuid, year, page, pageSize, status, startDate, endDate);

            if (absencesResult.absences == null || absencesResult.absences.Count == 0)
            {
                return NotFound();
            }

            var responseModels = absencesResult.absences.Select(a => a.ToAbsenceResponseModel()).ToList();

            var model = new AbsenceResponseModels()
            {
                Absences = responseModels,
                AllDataCount = absencesResult.count
            };

            // Return the list of AbsenceResponseModel objects and the total count of absences with HTTP status 200 OK
            return Ok(model);
        }


        /// <summary>
        /// Get all absence history for a specific absence.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page">The page</param>
        /// <param name="pageSize">The amount of data per page</param>
        /// <returns>AbsenceHistoryResponseModels object</returns>
        /// <response code="200">Returns AbsenceHistoryResponseModels object for the absence</response>
        /// <response code="404">If no absences are found for the person and year</response>
        [HttpGet("{id}/history")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AbsenceHistoryResponseModels>> GetAbsenceHistory(Guid id, int page, int pageSize)
        {
            if (page < DEFAULT_START_PAGE)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > 20)
            {
                pageSize = 20;
            }

            var absencesResult = await AbsenceRepository.GetAbsencesHistory(id, page, pageSize);

            var responseModels = absencesResult.absences.Select(a => a.ToAbsenceHistoryResponseModel()).ToList();

            if (responseModels == null || responseModels.Count == 0)
            {
                return NotFound();
            }

            var model = new AbsenceHistoryResponseModels()
            {
                AbsenceHistory = responseModels,
                AllDataCount = absencesResult.count
            };

            return Ok(model);
        }


        /// <summary>
        ///  Get a detailed AbsenceResponseModel object for a specific absence of a person.
        /// </summary>
        /// <param name="personGuid">The person ID</param>
        /// <param name="absenceId"></param>
        /// <returns>An AbsenceResponseModel object for the specific absence, or null if the absence does not exist</returns>
        /// <response code="200">Returns the AbsenceResponseModel object for the specific absence</response>
        /// <response code="404">If the absence does not exist</response>
        [HttpGet("{absenceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AbsenceResponseModel>> GetAbsenceByPerson(Guid personGuid, Guid absenceId)
        {
            var absence = await AbsenceRepository.GetAbsenceByPerson(personGuid, absenceId);
            if (absence == null)
            {
                return NotFound("Absence does not exist");
            }

            var responseModel = absence.ToAbsenceResponseModel();
            return Ok(responseModel);
        }

        /// <summary>
        /// Deletes an Absence.
        /// </summary>
        /// <param name="personGuid">The person ID</param>
        /// <param name="absenceId">The absence ID</param>
        /// <param name="actionBy">Who deleted</param>
        /// <returns></returns>
        /// <response code="204">Returns no content if the absence is deleted successfully</response>
        /// <response code="404">If the absence does not exist</response>
        [HttpDelete("{absenceId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAbsence(Guid personGuid, Guid absenceId, Guid actionBy)
        {
            // Delete the absence from the database
            var deleted = await AbsenceRepository.DeleteAbsence(absenceId, actionBy);

            if (!deleted)
            {
                return NotFound();
            }

            // Return HTTP status 204 No Content to indicate that the absence was deleted successfully
            return NoContent();
        }


        /// <summary>
        /// Update an existing absence.
        /// </summary>
        /// <param name="personGuid">The person ID</param>
        /// <param name="absenceId">The absence ID</param>
        /// <param name="updateModel">The updated AbsenceUpdateModel object</param>
        /// <param name="actionBy">Who updated</param>
        /// <returns></returns>
        /// <response code="204">Returns no content if the absence is updated successfully</response>
        /// <response code="404">If the absence does not exist</response>
        [HttpPut("{absenceId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateAbsence(Guid personGuid, Guid absenceId, [FromBody] AbsenceUpdateModel updateModel, Guid actionBy)
        {

            // Update the absence with the new values from the AbsenceUpdateModel object
            var repoModel = updateModel.ToAbsenceRepoModel();

            // Save the changes to the database
            var result = await AbsenceRepository.UpdateAbsence(repoModel, actionBy);

            if (!result)
            {
                return NotFound();
            }

            // Return HTTP status 204 No Content to indicate that the absence was updated successfully
            return NoContent();
        }





    }
}
