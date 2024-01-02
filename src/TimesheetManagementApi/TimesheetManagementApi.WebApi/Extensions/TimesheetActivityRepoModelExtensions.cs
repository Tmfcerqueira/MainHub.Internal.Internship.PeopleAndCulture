using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions
{
    public static class TimesheetActivityRepoModelExtensions
    {
        public static TimesheetActivityResponseModel ToTimesheetActivityResponseModel(this TimesheetActivityRepoModel model)
        {
            return new TimesheetActivityResponseModel
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
