using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Models;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;
using TimesheetManagement.DataBase.Models;

namespace TimesheetManagement.Repository.Extensions
{
    internal static class TimesheetActivityDbModelExtensions
    {
        public static TimesheetActivity ToTimesheetActivityDbModel(this TimesheetActivityRepoModel model)
        {
            return new TimesheetActivity
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

        public static TimesheetActivityHistory ToTimesheetActivityHistoryModel(this TimesheetActivityRepoModel model)
        {
            return new TimesheetActivityHistory
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
