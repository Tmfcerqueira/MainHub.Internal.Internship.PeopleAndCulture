using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;
using TimesheetManagement.DataBase.Models;

namespace TimesheetManagement.Repository.Extensions
{
    internal static class TimesheetActivityRepoModelExtensions
    {
        public static TimesheetActivityRepoModel ToTimesheetActivityRepoModel(this TimesheetActivity model)
        {
            return new TimesheetActivityRepoModel
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
