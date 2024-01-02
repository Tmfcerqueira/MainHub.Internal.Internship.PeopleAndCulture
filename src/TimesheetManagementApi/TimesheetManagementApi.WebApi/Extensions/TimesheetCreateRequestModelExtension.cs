using MainHub.Internal.PeopleAndCulture.Properties;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions
{
    public static class TimesheetCreateRequestModelExtension
    {
        public static TimesheetRepoModel ToTimesheetRepoModel(this TimesheetCreateRequestModel model)
        {
            return new TimesheetRepoModel
            {
                PersonGUID = model.PersonGUID,
                Month = model.Month,
                Year = model.Year
            };
        }
    }
}
