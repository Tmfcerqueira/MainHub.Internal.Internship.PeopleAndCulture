using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions
{
    public static class TimesheetHistoryRepoModelExtensions
    {
        public static TimesheetHistoryResponseModel ToTimesheetHistoryResponseModel(this TimesheetHistoryRepoModel model)
        {
            return new TimesheetHistoryResponseModel
            {
                PersonName = model.PersonName,
                PersonGUID = model.PersonGUID,
                TimesheetGUID = model.TimesheetGUID,
                Month = model.Month,
                Year = model.Year,
                DateOfApproval = model.DateOfApproval,
                DateOfSubmission = model.DateOfSubmission,
                ApprovalStatus = model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                ActionDate = model.ActionDate,
                Action = model.Action,
                UserGUID = model.UserGUID,
                ActionBy = model.ActionBy
            };
        }
    }
}
