using MainHub.Internal.PeopleAndCulture.Common;
using MainHub.Internal.PeopleAndCulture.Properties;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions
{
    public static class TimesheetActivityCreateRequestModelExtension
    {
        public static TimesheetActivityRepoModel ToTimesheetActivityRepoModel(this TimesheetActivityCreateRequestModel model)
        {
            return new TimesheetActivityRepoModel
            {
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
