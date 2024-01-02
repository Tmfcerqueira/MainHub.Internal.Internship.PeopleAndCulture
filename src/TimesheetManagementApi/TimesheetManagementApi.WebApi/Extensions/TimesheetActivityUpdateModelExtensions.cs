using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions
{
    public static class TimesheetActivityUpdateModelExtensions
    {
        public static TimesheetActivityRepoModel ToTimesheetActivityRepoModel(this TimesheetActivityUpdateModel model)
        {
            return new TimesheetActivityRepoModel()
            {
                TimesheetActivityGUID = model.TimesheetActivityGUID,
                ActivityGUID = model.ActivityGUID,
                TimesheetGUID = model.TimesheetGUID,
                ProjectGUID = model.ProjectGUID,
                TypeOfWork = model.TypeOfWork,
                ActivityDate = model.ActivityDate,
                Hours = model.Hours
            };
        }
    }
}
