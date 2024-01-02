using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TimesheetManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class ActivityRecordModelExtensions
    {
        public static TimesheetActivityCreateRequestModel ToActivityRecordCreateRequestModel(this TimesheetActivityRecordsModel model)
        {
            return new TimesheetActivityCreateRequestModel
            {
                ActivityGUID = model.ActivityGUID,
                TimesheetGUID = model.TimesheetGUID,
                ProjectGUID = model.ProjectGUID,
                TypeOfWork = (TypeOfWork?)model.TypeOfWork,
            };
        }

        public static TimesheetActivityRecordsModel ToActivityRecordsResponseModel(this TimesheetActivityResponseModel responseModel)
        {
            if (!responseModel.TypeOfWork.HasValue)
            {
                responseModel.TypeOfWork = TypeOfWork.Regular;
            }
            return new TimesheetActivityRecordsModel
            {
                ActivityGUID = responseModel.ActivityGUID,
                TimesheetGUID = responseModel.TimesheetGUID,
                ProjectGUID = responseModel.ProjectGUID,
                TypeOfWork = (Common.TypeOfWork)responseModel.TypeOfWork,
            };
        }

        public static List<TimesheetActivityModel> ToTimesheetActivitiesModel(this TimesheetActivityRecordsModel tarModel)
        {
            List<TimesheetActivityModel> modelList = new List<TimesheetActivityModel>();

            foreach (var recordmodel in tarModel.Days)
            {
                var model = new TimesheetActivityModel();
                model.TimesheetActivityGUID = recordmodel.TimesheetActivityGUID;
                model.ProjectGUID = tarModel.ProjectGUID;
                model.ActivityGUID = tarModel.ActivityGUID;
                model.TypeOfWork = tarModel.TypeOfWork;
                model.TimesheetGUID = tarModel.TimesheetGUID;
                model.Hours = recordmodel.Hours;
                model.ActivityDate = recordmodel.ActivityDate;
                modelList.Add(model);
            }

            return modelList;
        }
    }
}
