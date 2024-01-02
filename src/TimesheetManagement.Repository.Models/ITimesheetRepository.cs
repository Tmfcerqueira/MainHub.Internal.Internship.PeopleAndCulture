using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models
{
    public interface ITimesheetRepository
    {
        //Creates
        Task<TimesheetRepoModel> CreateTimesheet(TimesheetRepoModel timesheetRepoModel, Guid actionBy);
        Task<TimesheetActivityRepoModel> CreateTimesheetActivity(TimesheetActivityRepoModel timesheetActivityRepoModel, Guid actionBy);

        //Gets
        Task<(List<TimesheetRepoModel> timesheets, int count)> GetTimesheetsForPerson(Guid personGuid, int year, int month, ApprovalStatus approvalStatus, int page, int pageSize);
        Task<TimesheetRepoModel?> GetOneTimesheetForPerson(Guid personGuid, Guid timesheetGuid);

        Task<List<TimesheetActivityRepoModel>> GetTimesheetActivitiesForTimesheet(Guid timesheetGuid);
        Task<TimesheetActivityRepoModel?> GetOneTimesheetActivityForTimesheet(Guid timesheetGuid, Guid timesheetActivityGuid);

        //Deletes
        Task<bool> DeleteTimesheetActivity(Guid timesheetActivityGuid, Guid actionBy);

        //Updates
        Task<bool> UpdateTimesheet(TimesheetRepoModel updatedtimesheet, Guid actionBy);
        Task<bool> UpdateTimesheetActivity(TimesheetActivityRepoModel updatedtimesheetActivity, Guid actionBy);

        //History
        Task<(List<TimesheetHistoryRepoModel> timesheets, int count)> GetTimesheetHistory(Guid timesheetGuid, int page, int pageSize);
        Task<List<TimesheetActivityHistoryRepoModel>> GetTimesheetActivityHistory(Guid timesheetGuid, Guid timesheetActivityGuid, int page, int pageSize);

        //Admin
        Task<(List<TimesheetRepoModel> timesheets, int totalCount)> GetAllTimesheets(int page, int pageSize, ApprovalStatus status);
    }
}
