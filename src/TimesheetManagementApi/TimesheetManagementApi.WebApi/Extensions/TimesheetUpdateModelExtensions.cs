using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions
{
    public static class TimesheetUpdateModelExtensions
    {
        public static TimesheetRepoModel ToTimesheetRepoModel(this TimesheetUpdateModel model)
        {
            return new TimesheetRepoModel()
            {
                TimesheetGUID = model.TimesheetGUID,
                PersonGUID = model.PersonGUID,
                Year = model.Year,
                Month = model.Month,
                ApprovalStatus = model.ApprovalStatus,
                DateOfSubmission = model.DateOfSubmission,
                DateOfApproval = model.DateOfApproval,
                ApprovedBy = model.ApprovedBy,
            };
        }
    }
}
