using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimesheetManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.Common;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class TimesheetActivityRepoModelExtensions
    {
        public static TimesheetActivityModel ToTimesheetActivityModel(this TimesheetActivityResponseModel model)
        {
            if (!model.TypeOfWork.HasValue)
            {
                model.TypeOfWork = (TimesheetManagement.Api.Proxy.Client.Model.TypeOfWork?)Common.TypeOfWork.Regular;
            }

            return new TimesheetActivityModel
            {
                TimesheetActivityGUID = model.TimesheetActivityGUID,
                ActivityGUID = model.ActivityGUID,
                TimesheetGUID = model.TimesheetGUID,
                ProjectGUID = model.ProjectGUID,
                TypeOfWork = (Common.TypeOfWork)model.TypeOfWork,
                ActivityDate = model.ActivityDate,
                Hours = model.Hours
            };
        }
    }
}
