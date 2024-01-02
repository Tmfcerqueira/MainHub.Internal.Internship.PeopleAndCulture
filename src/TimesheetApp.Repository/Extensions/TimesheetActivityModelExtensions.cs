using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TimesheetManagement.Api.Proxy.Client.Api;
using TimesheetManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class TimesheetActivityModelExtensions
    {
        public static TimesheetActivityCreateRequestModel ToTimesheetActivityCreateRequestModel(this TimesheetActivityModel model)
        {
            return new TimesheetActivityCreateRequestModel
            {
                ActivityGUID = model.ActivityGUID,
                TimesheetGUID = model.TimesheetGUID,
                ProjectGUID = model.ProjectGUID,
                TypeOfWork = (TypeOfWork?)model.TypeOfWork,
                ActivityDate = model.ActivityDate,
                Hours = model.Hours
            };
        }

        public static TimesheetActivityModel ToTimesheetActivityResponseModel(this TimesheetActivityCreateResponseModel responseModel)
        {
            if (!responseModel.TypeOfWork.HasValue)
            {
                responseModel.TypeOfWork = TypeOfWork.Regular;
            }
            return new TimesheetActivityModel
            {
                TimesheetActivityGUID = responseModel.TimesheetActivityGUID,
                ActivityGUID = responseModel.ActivityGUID,
                TimesheetGUID = responseModel.TimesheetGUID,
                ProjectGUID = responseModel.ProjectGUID,
                TypeOfWork = (Common.TypeOfWork)responseModel.TypeOfWork,
                ActivityDate = responseModel.ActivityDate,
                Hours = responseModel.Hours
            };
        }

        public static TimesheetActivityUpdateModel ToTimesheetActivityUpdateModel(this TimesheetActivityModel model)
        {
            return new TimesheetActivityUpdateModel
            {
                TimesheetActivityGUID = model.TimesheetActivityGUID,
                ActivityGUID = model.ActivityGUID,
                ProjectGUID = model.ProjectGUID,
                TimesheetGUID = model.TimesheetGUID,
                TypeOfWork = (TypeOfWork?)model.TypeOfWork,
                ActivityDate = model.ActivityDate,
                Hours = model.Hours
            };
        }
    }
}
