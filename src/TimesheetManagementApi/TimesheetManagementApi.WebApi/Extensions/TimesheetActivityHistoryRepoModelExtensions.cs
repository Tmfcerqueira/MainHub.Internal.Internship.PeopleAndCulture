using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions
{
    public static class TimesheetActivityHistoryRepoModelExtensions
    {
        public static TimesheetActivityHistoryResponseModel ToTimesheetActivityHistoryResponseModel(this TimesheetActivityHistoryRepoModel model)
        {
            return new TimesheetActivityHistoryResponseModel
            {
                PersonName = model.PersonName,
                TimesheetActivityGUID = model.TimesheetActivityGUID,
                TimesheetGUID = model.TimesheetGUID,
                ActivityGUID = model.ActivityGUID,
                ProjectGUID = model.ProjectGUID,
                TypeOfWork = model.TypeOfWork,
                ActivityDate = model.ActivityDate,
                Hours = model.Hours,
                ActionDate = model.ActionDate,
                Action = model.Action,
                UserGUID = model.UserGUID,
                ActionBy = model.ActionBy
            };
        }
    }
}
