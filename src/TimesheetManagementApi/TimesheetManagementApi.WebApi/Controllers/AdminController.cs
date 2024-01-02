using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;
using MainHub.Internal.PeopleAndCulture.Common;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Controllers
{
    [Authorize(Roles = "SkillHub.Supervisor")]
    [ApiController]
    [Route("api/timesheet")]
    public class AdminController : ControllerBase
    {
        private const int DEFAULT_START_PAGE = 1;
        private const int DEFAULT_PAGE_SIZE = 20;
        public readonly ITimesheetRepository TimesheetRepository;

        public AdminController(ITimesheetRepository timesheetRepository)
        {
            this.TimesheetRepository = timesheetRepository;
        }

        /// <summary>
        /// Get All Timesheets.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns>TimesheetResponseModels</returns>
        /// <response code="200">Returns success if it got the timesheets</response>
        /// <response code="404">If timesheets does not exist</response>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TimesheetResponseModels>> GetAllTimesheets(int page, int pageSize, ApprovalStatus status)
        {
            if (page < DEFAULT_START_PAGE)
            {
                page = 1;
            }

            if (pageSize > DEFAULT_PAGE_SIZE || pageSize < 1)
            {
                pageSize = 20;
            }

            //Get the timesheets, but only retrieve the page of items
            var timesheetsResult = await TimesheetRepository.GetAllTimesheets(page, pageSize, status);

            if (timesheetsResult.timesheets == null || timesheetsResult.timesheets.Count == 0)
            {
                return NotFound();
            }

            var responseModels = timesheetsResult.timesheets.Select(a => a.ToTimesheetResponseModel()).ToList();

            var model = new TimesheetResponseModels()
            {
                Timesheets = responseModels,
                AllDataCount = timesheetsResult.totalCount
            };

            //Return the list of TimesheetResponseModel objects and the total count of timesheets with HTTP status 200 OK
            return Ok(model);
        }
    }
}
