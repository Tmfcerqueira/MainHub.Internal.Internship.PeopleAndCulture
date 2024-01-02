using MainHub.Internal.PeopleAndCulture.TimesheetManagement;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Repository;
using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.AspNetCore.Mvc;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;
using MainHub.Internal.PeopleAndCulture.Properties;
using Microsoft.AspNetCore.Authorization;
using System.Drawing.Printing;
using TimesheetManagement.DataBase.Models;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Controllers
{
    [ApiController]
    [Route("/api/people/{personGuid}/timesheet")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = "SkillHub.User")]
    public class TimesheetController : Controller
    {
        private const int DEFAULT_START_PAGE = 1;
        private const int DEFAULT_PAGE_SIZE = 20;
        public readonly ITimesheetRepository TimesheetRepository;

        public TimesheetController(ITimesheetRepository timesheetRepository)
        {
            this.TimesheetRepository = timesheetRepository;
        }

        /// <summary>
        /// Creates a Timesheet.
        /// </summary>
        /// <param name = "model" > The model used to create a Timesheet.</param>
        /// <param name = "personGuid" > Used to define the person of the Timesheet.</param>
        /// <param name = "actionBy" > Used to see the person who created the Timesheet.</param>
        /// <returns></returns>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TimesheetCreateResponseModel>> CreateTimesheet(Guid personGuid, TimesheetCreateRequestModel model, Guid actionBy)
        {
            if (model.PersonGUID != personGuid)
            {
                return BadRequest(model);
            }

            var repoModel = model.ToTimesheetRepoModel();

            //Save the timesheet entity to the database
            var createResponse = await TimesheetRepository.CreateTimesheet(repoModel, actionBy);

            //Create the response model from the saved timesheet entity
            var response = createResponse.ToTimesheetCreateResponseModel();

            //Return the response model with HTTP status 200 OK
            return Ok(response);
        }


        /// <summary>
        /// Gets all the Timesheets of a specific person.
        /// </summary>
        /// <param name = "personGuid" > Used to get the person Timesheets.</param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="approvalStatus"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <response code="200">Returns all the timesheets</response>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TimesheetResponseModels>> GetAllTimesheetsForPerson(
            Guid personGuid,
            int year,
            int month,
            ApprovalStatus approvalStatus,
            int page,
            int pageSize)
        {
            if (page < DEFAULT_START_PAGE)
            {
                page = 1;
            }

            if (pageSize > DEFAULT_PAGE_SIZE || pageSize < 1)
            {
                pageSize = 20;
            }

            var timesheetsResult = await TimesheetRepository.GetTimesheetsForPerson(personGuid, year, month, approvalStatus, page, pageSize);

            var responseModels = timesheetsResult.timesheets.Select(t => t.ToTimesheetResponseModel()).ToList();

            if (responseModels == null || responseModels.Count == 0)
            {
                return NotFound();
            }

            var model = new TimesheetResponseModels()
            {
                Timesheets = responseModels,
                AllDataCount = timesheetsResult.count
            };

            return Ok(model);
        }

        /// <summary>
        /// Gets one specific Timesheet of a person.
        /// </summary>
        ///<param name = "personGuid" > Used to get the person Timesheets.</param>
        ///<param name = "timesheetGuid" > Used to get the specific Timesheet</param>
        /// <returns></returns>
        /// <response code="200">Returns the timesheet</response>
        /// <response code="400">If the item is null</response>
        [HttpGet("{timesheetGuid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TimesheetResponseModel>> GetTimesheetForPerson(Guid personGuid, Guid timesheetGuid)
        {
            var timesheet = await TimesheetRepository.GetOneTimesheetForPerson(personGuid, timesheetGuid);

            var responseModel = timesheet?.ToTimesheetResponseModel();

            if (responseModel == null)
            {
                return NotFound();
            }
            return Ok(responseModel);
        }



        /// <summary>
        /// Updates a Timesheet.
        /// </summary>
        /// <param name = "personGuid"> Used to get the timesheets of the person</param>
        /// <param name = "timesheetGuid"> Used to get the specific timesheet</param>
        /// <param name = "updateModel">The updated TimesheetUpdateModel object</param>
        /// <param name = "actionBy"> Used to see the person who updated the Timesheet.</param>
        /// <returns></returns>
        /// <response code="204">Returns no content if the timesheet is updated successfully</response>
        /// <response code="404">If the timesheet does not exist</response>
        [HttpPut("{timesheetGuid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateTimesheet(Guid personGuid, Guid timesheetGuid, [FromBody] TimesheetUpdateModel updateModel, Guid actionBy)
        {
            //Get the timesheet
            var timesheet = await TimesheetRepository.GetOneTimesheetForPerson(personGuid, timesheetGuid);

            if (timesheet == null)
            {
                return NotFound();
            }

            //Update the timesheet with the new values
            var repoModel = updateModel.ToTimesheetRepoModel();

            await TimesheetRepository.UpdateTimesheet(repoModel, actionBy);

            return NoContent();
        }



        /// <summary>
        /// Get the history for a specific timesheet.
        /// </summary>
        /// <param name="timesheetGuid">Used to get the specific timesheet</param>
        /// <param name="page">The page</param>
        /// <param name="pageSize">The amount of data per page</param>
        /// <returns>TimesheetHistoryResponseModels object</returns>
        /// <response code="200">Returns TimesheetHistoryResponseModels object for the timesheet</response>
        /// <response code="404">If no timesheets are found</response>
        [HttpGet("{timesheetGuid}/history")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TimesheetHistoryResponseModels>> GetTimesheetHistory(Guid timesheetGuid, int page, int pageSize)
        {
            if (page < DEFAULT_START_PAGE)
            {
                page = 1;
            }

            if (pageSize > DEFAULT_PAGE_SIZE || pageSize < 1)
            {
                pageSize = 20;
            }

            var timesheetResult = await TimesheetRepository.GetTimesheetHistory(timesheetGuid, page, pageSize);

            var responseModels = timesheetResult.timesheets.Select(a => a.ToTimesheetHistoryResponseModel()).ToList();

            if (responseModels == null || responseModels.Count == 0)
            {
                return NotFound();
            }

            var model = new TimesheetHistoryResponseModels()
            {
                TimesheetHistory = responseModels,
                AllDataCount = responseModels.Count
            };

            return Ok(model);
        }
    }
}
