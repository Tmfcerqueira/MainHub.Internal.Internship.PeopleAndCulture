using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimesheetManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture
{
    public interface ITimesheetAppRepository
    {
        //Creates
        Task<TimesheetModel> CreateTimesheet(TimesheetModel timesheetModel, Guid actionBy);

        Task<List<TimesheetActivityModel>> CreateActivities(Guid personGuid, Guid timesheetActivityGuid, Guid actionBy, TimesheetActivityRecordsModel tarm);

        //Gets
        Task<(List<TimesheetModel>, int errorCode, int count)> GetAllTimesheetsFromOnePerson(Guid personGuid, int year, int month, ApprovalStatus approvalStatus, int page, int pageSize);
        Task<TimesheetModel> GetOneTimesheetFromOnePerson(Guid personGuid, Guid timesheetGuid);
        Task<(List<TimesheetActivityModel>, int errorCode, int count)> GetAllTimesheetActivitiesForOneTimesheet(Guid personGuid, Guid timesheetGuid);
        Task<TimesheetActivityModel> GetOneTimesheetActivityForOneTimesheet(Guid personGuid, Guid timesheetGuid, Guid timesheetActivityGuid);

        Task<List<ProjectModel>> GetProjectsForUser(Guid personGuid);
        Task<List<ProjectActivityModel>> GetProjectActivities(Guid personGuid, Guid projectGuid);

        Task<(List<TimesheetActivityRecordsModel>, int errorCode, int count)> GetTimesheetActivityRecords(Guid personGuid, Guid timesheetGuid);

        //Deletes
        Task<bool> DeleteTimesheetActivityAsync(Guid personGuid, Guid timesheetGuid, Guid timesheetActivityGuid, Guid actionBy);

        //Updates
        Task<bool> UpdateTimesheetAsync(Guid personGuid, Guid timesheetGuid, TimesheetModel updatedModel, Guid actionBy);

        //History
        Task<(List<TimesheetHistoryModel>, int errorCode, int count)> GetTimesheetHistory(Guid timesheetGuid, string personGuid, int page, int pageSize);

        //Admin
        Task<(List<TimesheetModel>, int errorCode, int count)> GetAllTimesheetsAsync(int page, int pageSize, ApprovalStatus status);
    }
}
