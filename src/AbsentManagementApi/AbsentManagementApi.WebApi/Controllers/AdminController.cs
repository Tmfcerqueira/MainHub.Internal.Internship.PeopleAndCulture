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
    [Authorize(Roles = "SkillHub.Supervisor")]
    [ApiController]
    [Route("api/absence")]
    public class AdminController : ControllerBase
    {
        private const int DEFAULT_START_PAGE = 1;
        public readonly IAbsenceRepository AbsenceRepository;

        public AdminController(IAbsenceRepository absenceRepository)
        {
            this.AbsenceRepository = absenceRepository;
        }

        //Admin

        /// <summary>
        /// Get All Absences.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns>AbsenceResponseModels</returns>
        /// <response code="200">Returns success if it got the absences successfully</response>
        /// <response code="404">If absences does not exist</response>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AbsenceResponseModels>> GetAllAbsences(int page, int pageSize, ApprovalStatus status)
        {
            if (page < DEFAULT_START_PAGE)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > 20)
            {
                pageSize = 20;
            }

            // Get the absences, but only retrieve the page of items
            var absencesResult = await AbsenceRepository.GetAllAbsences(page, pageSize, status);

            if (absencesResult.absences == null || absencesResult.absences.Count == 0)
            {
                return NotFound();
            }

            var responseModels = absencesResult.absences.Select(a => a.ToAbsenceResponseModel()).ToList();

            var model = new AbsenceResponseModels()
            {
                Absences = responseModels,
                AllDataCount = absencesResult.totalCount
            };

            // Return the list of AbsenceResponseModel objects and the total count of absences with HTTP status 200 OK
            return Ok(model);
        }
    }
}
