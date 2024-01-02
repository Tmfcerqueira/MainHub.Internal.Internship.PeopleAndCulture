using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions
{
    public static class TimesheetCreateResponseModelExtension
    {
        public static TimesheetCreateResponseModel ToTimesheetCreateResponseModel(this TimesheetRepoModel model)
        {
            return new TimesheetCreateResponseModel
            {
                TimesheetGUID = model.TimesheetGUID,
                PersonGUID = model.PersonGUID,
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
