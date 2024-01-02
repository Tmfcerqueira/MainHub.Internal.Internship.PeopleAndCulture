using MainHub.Internal.PeopleAndCulture.Common;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions
{
    public static class TimesheetActivityCreateResponseModelExtension
    {
        public static TimesheetActivityCreateResponseModel ToTimesheetActivityCreateResponseModel(this TimesheetActivityRepoModel model)
        {
            return new TimesheetActivityCreateResponseModel
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
