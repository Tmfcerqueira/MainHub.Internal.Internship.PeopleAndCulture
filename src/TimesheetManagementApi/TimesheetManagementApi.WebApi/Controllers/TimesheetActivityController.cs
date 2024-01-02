using System.Drawing.Printing;
using MainHub.Internal.PeopleAndCulture.Common;
using MainHub.Internal.PeopleAndCulture.Properties;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimesheetManagement.DataBase.Models;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Controllers
{
    [ApiController]
    [Route("/api/people/{personGuid}/timesheet/{timesheetGuid}/activity")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = "SkillHub.User")]
    public class TimesheetActivityController : Controller
    {
        public readonly ITimesheetRepository TimesheetActivityRepository;

        public TimesheetActivityController(ITimesheetRepository timesheetActivityRepository)
        {
            this.TimesheetActivityRepository = timesheetActivityRepository;
        }

        /// <summary>
        /// Creates a Timesheet Activity.
        /// </summary>
        /// <param name = "model" > The model used to create a TimesheetActivity.</param>
        /// <param name = "personGuid" > Used to define the person of the timesheet and its activities.</param>
        /// <param name = "timesheetGuid"> Used to associate the Timesheet with its Timesheet Activities</param>
        /// <param name = "actionBy"> Used to see the person who created the Timesheet Activity.</param>
        /// <returns></returns>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TimesheetActivityCreateResponseModel>> CreateTimesheetActivity(Guid personGuid, Guid timesheetGuid, TimesheetActivityCreateRequestModel model, Guid actionBy)
        {
            if (model.TimesheetGUID != timesheetGuid)
            {
                return BadRequest(model);
            }

            var repoModel = model.ToTimesheetActivityRepoModel();

            //Save the timesheetActivity entity to the database
            var createResponse = await TimesheetActivityRepository.CreateTimesheetActivity(repoModel, actionBy);

            //Create the response model from the saved timesheetActivity entity
            var response = createResponse.ToTimesheetActivityCreateResponseModel();

            //Return the response model with HTTP status 200 OK
            return Ok(response);
        }


        /// <summary>
        /// Gets all the Timesheet Activities of a specific Timesheet.
        /// </summary>
        /// <param name="personGuid"> Used to get the timesheets of the person.</param>
        ///<param name = "timesheetGuid" > Used to get the Timesheet Activities.</param>
        /// <returns></returns>
        /// <response code="200">Returns all the timesheet activities</response>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TimesheetActivityResponseModels>> GetAllTimesheetActivities(Guid personGuid, Guid timesheetGuid)
        {
            var timesheetActivities = await TimesheetActivityRepository.GetTimesheetActivitiesForTimesheet(timesheetGuid);

            var responseModels = timesheetActivities.Select(a => a.ToTimesheetActivityResponseModel()).ToList();

            if (responseModels == null || responseModels.Count == 0)
            {
                return NotFound();
            }

            var model = new TimesheetActivityResponseModels()
            {
                Activities = responseModels,
                AllDataCount = timesheetActivities.Count
            };

            return Ok(model);
        }

        /// <summary>
        /// Gets one Timesheet Activity of a specific timesheet.
        /// </summary>
        /// <param name="personGuid"> Used to get the timesheets of the person.</param>
        ///<param name = "timesheetGuid" > Used to get the Timesheet Activities.</param>
        ///<param name = "timesheetActivityGuid" > Used to get the specific Activity.</param>
        /// <returns></returns>
        /// <response code="200">Returns the timesheet activity</response>
        /// <response code="400">If the item is null</response>
        [HttpGet("{timesheetActivityGuid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TimesheetActivityResponseModel>> GetTimesheetActivityForTimesheet(Guid personGuid, Guid timesheetGuid, Guid timesheetActivityGuid)
        {
            var timesheetActivity = await TimesheetActivityRepository.GetOneTimesheetActivityForTimesheet(timesheetGuid, timesheetActivityGuid);

            var responseModel = timesheetActivity?.ToTimesheetActivityResponseModel();

            if (responseModel == null)
            {
                return NotFound();
            }
            return Ok(responseModel);
        }


        /// <summary>
        /// Deletes a Timesheet Activity.
        /// </summary>
        /// <param name = "personGuid"> Used to get the timesheets of the person</param>
        /// <param name = "timesheetGuid"> Used to indicate the timesheet of the activity</param>
        /// <param name = "timesheetActivityGuid"> Used to indicate the timesheet activity that is going to be deleted</param>
        /// <param name = "actionBy"> Used to see the person who deleted the Timesheet Activity.</param>
        /// <returns></returns>
        /// <response code="204">Returns no content if the timesheet activity is deleted successfully</response>
        /// <response code="404">If the timesheet activity does not exist</response>
        [HttpDelete("{timesheetActivityGuid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTimesheetActivity(Guid personGuid, Guid timesheetGuid, Guid timesheetActivityGuid, Guid actionBy)
        {
            //Delete the activity from the database
            var deleted = await TimesheetActivityRepository.DeleteTimesheetActivity(timesheetActivityGuid, actionBy);

            if (!deleted)
            {
                return NotFound();
            }

            //Return HTTP status 204 No Content to indicate that the activity was deleted successfully
            return NoContent();
        }



        /// <summary>
        /// Updates a Timesheet Activity.
        /// </summary>
        /// <param name = "personGuid"> Used to get the timesheets of the person</param>
        /// <param name = "timesheetGuid"> Used to indicate the timesheet of the activity</param>
        /// <param name = "timesheetActivityGuid"> Used to indicate the timesheet activity that is going to be updated</param>
        /// <param name = "updateModel">The updated TimesheetActivityUpdateModel object</param>
        /// <param name = "actionBy"> Used to see the person who updated the Timesheet Activity.</param>
        /// <returns></returns>
        /// <response code="204">Returns no content if the timesheet activity is updated successfully</response>
        /// <response code="404">If the timesheet activity does not exist</response>
        [HttpPut("{timesheetActivityGuid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateTimesheetActivity(Guid personGuid, Guid timesheetGuid, Guid timesheetActivityGuid, [FromBody] TimesheetActivityUpdateModel updateModel, Guid actionBy)
        {
            //Get the timesheet activity
            var timesheetActivity = await TimesheetActivityRepository.GetOneTimesheetActivityForTimesheet(timesheetGuid, timesheetActivityGuid);

            if (timesheetActivity == null)
            {
                return NotFound();
            }

            //Update the timesheet activity with the new values
            var repoModel = updateModel.ToTimesheetActivityRepoModel();

            await TimesheetActivityRepository.UpdateTimesheetActivity(repoModel, actionBy);

            return NoContent();
        }



        /// <summary>
        /// Get the history for a specific timesheet activity.
        /// </summary>
        /// <param name="timesheetGuid">Used to get the specific timesheet</param>
        /// <param name="timesheetActivityGuid">Used to get the specific timesheet activity</param>
        /// <param name="page">The page</param>
        /// <param name="pageSize">The amount of data per page</param>
        /// <returns>TimesheetActivityHistoryResponseModels object</returns>
        /// <response code="200">Returns TimesheetActivityHistoryResponseModels object for the timesheet activity</response>
        /// <response code="404">If no timesheets activities are found</response>
        [HttpGet("{timesheetActivityGuid}/history")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TimesheetActivityHistoryResponseModels>> GetTimesheetActivityHistory(Guid timesheetGuid, Guid timesheetActivityGuid, int page, int pageSize)
        {
            var timesheetActivityResult = await TimesheetActivityRepository.GetTimesheetActivityHistory(timesheetGuid, timesheetActivityGuid, page, pageSize);

            var responseModels = timesheetActivityResult.Select(a => a.ToTimesheetActivityHistoryResponseModel()).ToList();

            if (responseModels == null || responseModels.Count == 0)
            {
                return NotFound();
            }

            var model = new TimesheetActivityHistoryResponseModels()
            {
                TimesheetActivityHistory = responseModels,
                AllDataCount = timesheetActivityResult.Count
            };

            return Ok(model);
        }
    }
}
