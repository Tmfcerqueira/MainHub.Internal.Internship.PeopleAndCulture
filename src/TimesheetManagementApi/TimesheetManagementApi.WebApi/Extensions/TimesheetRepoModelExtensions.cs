using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions
{
    public static class TimesheetRepoModelExtensions
    {
        public static TimesheetResponseModel ToTimesheetResponseModel(this TimesheetRepoModel model)
        {
            return new TimesheetResponseModel
            {
                TimesheetGUID = model.TimesheetGUID,
                PersonGUID = model.PersonGUID,
                PersonName = model.PersonName,
                Month = model.Month,
                Year = model.Year,
                ApprovalStatus = model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                DateOfSubmission = model.DateOfSubmission,
                DateOfApproval = model.DateOfApproval
            };
        }
    }
}
