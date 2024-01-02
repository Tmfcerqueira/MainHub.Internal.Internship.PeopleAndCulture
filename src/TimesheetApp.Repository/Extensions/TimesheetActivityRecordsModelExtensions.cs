using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TimesheetManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class TimesheetActivityRecordsModelExtensions
    {
        public static TimesheetActivityRecordsModel ToTimesheetActivityRecordsModel(this TimesheetActivityModel timesheetActivityModel)
        {
            var model = new TimesheetActivityRecordsModel(0, timesheetActivityModel.ActivityDate.Year, timesheetActivityModel.ActivityDate.Month, Guid.Empty);
            model.TimesheetActivityGUID = timesheetActivityModel.TimesheetActivityGUID;
            model.TimesheetGUID = timesheetActivityModel.TimesheetGUID;
            model.ProjectGUID = timesheetActivityModel.ProjectGUID;
            model.ActivityGUID = timesheetActivityModel.ActivityGUID;
            model.TypeOfWork = timesheetActivityModel.TypeOfWork;
            return model;
        }


        public static List<TimesheetActivityRecordsModel> TimesheetActivityRecords(List<TimesheetActivityModel> activityModels)
        {
            var records = activityModels.OrderBy(x => x.ProjectGUID).ThenBy(x => x.ActivityGUID).ThenBy(x => x.TypeOfWork).ToList();
            var recordsModel = new List<TimesheetActivityRecordsModel>();
            Guid project = Guid.Empty;
            Guid activityGUID = Guid.Empty;
            Common.TypeOfWork work = Common.TypeOfWork.Regular;


            TimesheetActivityRecordsModel recordModel = new TimesheetActivityRecordsModel(0, records[0].ActivityDate.Year, records[0].ActivityDate.Month, Guid.Empty);

            foreach (var record in records)
            {
                if (record.ProjectGUID != project || record.ActivityGUID != activityGUID || !record.TypeOfWork.Equals(work))
                {
                    recordModel = record.ToTimesheetActivityRecordsModel();
                    recordsModel.Add(recordModel);
                    project = record.ProjectGUID;
                    activityGUID = record.ActivityGUID;
                    work = record.TypeOfWork;
                }


                if (record.ProjectGUID == project && record.ActivityGUID == activityGUID && record.TypeOfWork.Equals(work))
                {
                    recordModel.Days[record.ActivityDate.Day - 1] = new ActivityDayModel(record.Hours, record.ActivityDate, record.TimesheetActivityGUID);
                }
            }
            return recordsModel;
        }
    }
}
