using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;
using MainHub.Internal.PeopleAndCulture.Extensions;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;
using Microsoft.EntityFrameworkCore;
using PeopleManagementDataBase;
using TimesheetManagement.DataBase;
using TimesheetManagement.DataBase.Models;
using TimesheetManagement.Repository.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.Repository
{
    public class TimesheetRepository : ITimesheetRepository
    {
        public readonly TimesheetManagementDbContext DbContext;

        public TimesheetRepository(TimesheetManagementDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private static TimesheetRepoModel MapTimesheetToRepoModel(Timesheet timesheet, Collaborator person)
        {
            return new TimesheetRepoModel
            {
                PersonGUID = person.PeopleGUID,
                PersonName = $"{person.FirstName} {person.LastName}",
                TimesheetGUID = timesheet.TimesheetGUID,
                Month = timesheet.Month,
                Year = timesheet.Year,
                DateOfSubmission = timesheet.DateOfSubmission,
                DateOfApproval = timesheet.DateOfApproval,
                ApprovalStatus = timesheet.ApprovalStatus,
                ApprovedBy = $"{person.FirstName} {person.LastName}",
                CreatedBy = timesheet.CreatedBy,
                EditedBy = timesheet.EditedBy,
                DeletedBy = timesheet.DeletedBy,
                IsDeleted = timesheet.IsDeleted,
            };
        }


        //Creates
        public async Task<TimesheetRepoModel> CreateTimesheet(TimesheetRepoModel timesheetRepoModel, Guid actionBy)
        {
            //Mapper TimesheetRepoModel to Timesheet
            var timesheet = timesheetRepoModel.ToTimesheetDbModel();

            var timesheetHistory = timesheetRepoModel.ToTimesheetHistoryModel();

            timesheet.TimesheetGUID = Guid.NewGuid();
            timesheet.CreatedBy = actionBy;

            timesheetHistory.TimesheetGUID = timesheet.TimesheetGUID;
            timesheetHistory.Action = "Create";
            timesheetHistory.ActionDate = DateTime.Now;
            timesheetHistory.UserGUID = actionBy;

            DbContext.Timesheet.Add(timesheet);
            DbContext.TimesheetHistory.Add(timesheetHistory);
            await DbContext.SaveChangesAsync();

            return timesheet.ToTimesheetRepoModel();
        }

        public async Task<TimesheetActivityRepoModel> CreateTimesheetActivity(TimesheetActivityRepoModel timesheetActivityRepoModel, Guid actionBy)
        {
            //Mapper TimesheetActivityRepoModel to TimesheetActivity
            var timesheetActivity = timesheetActivityRepoModel.ToTimesheetActivityDbModel();

            timesheetActivity.TimesheetActivityGUID = Guid.NewGuid();
            timesheetActivity.CreatedBy = actionBy;

            var timesheetActivityHistory = timesheetActivityRepoModel.ToTimesheetActivityHistoryModel();

            timesheetActivityHistory.TimesheetActivityGUID = timesheetActivity.TimesheetActivityGUID;
            timesheetActivityHistory.Action = "Created";
            timesheetActivityHistory.ActionDate = DateTime.Now;
            timesheetActivityHistory.UserGUID = actionBy;

            DbContext.TimesheetActivity.Add(timesheetActivity);
            DbContext.TimesheetActivityHistory.Add(timesheetActivityHistory);
            await DbContext.SaveChangesAsync();

            return timesheetActivity.ToTimesheetActivityRepoModel();
        }

        //Gets
        public async Task<(List<TimesheetRepoModel> timesheets, int count)> GetTimesheetsForPerson(Guid personGuid, int year, int month, ApprovalStatus approvalStatus, int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            int take = pageSize;

            var query = DbContext.Timesheet
                .Where(
                    a => a.PersonGUID == personGuid && !a.IsDeleted);
            if (year > 0)
            {
                query.Where(a => a.Year == year);
            }
            if (month > 0)
            {
                query.Where(a => a.Month == month);
            }
            if (approvalStatus != ApprovalStatus.All)
            {
                query.Where(a => a.ApprovalStatus == approvalStatus);
            }

            var totalCount = await query.CountAsync();

            var timesheets = query.OrderByDescending(a => a.Year).ThenBy(a => a.Month)
                         .Skip(skip)
                         .Take(take);

            return (timesheets.Select(a => a.ToTimesheetRepoModel()).ToList(), totalCount);
        }

        public async Task<TimesheetRepoModel?> GetOneTimesheetForPerson(Guid personGuid, Guid timesheetGuid)
        {
            var timesheet = await DbContext.Timesheet
                .Where(a => a.PersonGUID == personGuid && a.TimesheetGUID == timesheetGuid && !a.IsDeleted)
                .FirstOrDefaultAsync();

            if (timesheet == null)
            {
                return null;
            }

            return timesheet.ToTimesheetRepoModel();
        }

        public async Task<List<TimesheetActivityRepoModel>> GetTimesheetActivitiesForTimesheet(Guid timesheetGuid)
        {
            var activities = await DbContext.TimesheetActivity
                .Where(a => a.TimesheetGUID == timesheetGuid && !a.IsDeleted)
                .OrderBy(c => c.ProjectGUID)
                .ThenBy(c => c.ActivityGUID)
                .ThenBy(c => c.TypeOfWork)
                .ToListAsync();

            return activities.Select(a => a.ToTimesheetActivityRepoModel()).ToList();
        }

        public async Task<TimesheetActivityRepoModel?> GetOneTimesheetActivityForTimesheet(Guid timesheetGuid, Guid timesheetActivityGuid)
        {
            var activity = await DbContext.TimesheetActivity
                .Where(a => a.TimesheetGUID == timesheetGuid && a.TimesheetActivityGUID == timesheetActivityGuid && !a.IsDeleted)
                .FirstOrDefaultAsync();

            if (activity == null)
            {
                return null;
            }

            return activity.ToTimesheetActivityRepoModel();
        }

        //Deletes
        public async Task<bool> DeleteTimesheetActivity(Guid timesheetActivityGuid, Guid actionBy)
        {
            var timesheetActivity = await DbContext.TimesheetActivity.FirstOrDefaultAsync(a => a.TimesheetActivityGUID == timesheetActivityGuid);
            if (timesheetActivity == null)
            {
                return false;
            }

            timesheetActivity.DeletedBy = actionBy;
            timesheetActivity.IsDeleted = true;

            var timesheetActivityHistory = timesheetActivity.ToTimesheetActivityRepoModel().ToTimesheetActivityHistoryModel();

            timesheetActivityHistory.Action = "Delete";
            timesheetActivityHistory.ActionDate = DateTime.Now;
            timesheetActivityHistory.UserGUID = actionBy;

            await DbContext.TimesheetActivityHistory.AddAsync(timesheetActivityHistory);
            await DbContext.SaveChangesAsync();

            return true;
        }


        //Updates
        public async Task<bool> UpdateTimesheet(TimesheetRepoModel updatedtimesheet, Guid actionBy)
        {
            var timesheet = await DbContext.Timesheet.FirstOrDefaultAsync(a => a.TimesheetGUID == updatedtimesheet.TimesheetGUID);
            if (timesheet == null)
            {
                return false; //timesheet not found
            }

            timesheet.ApprovalStatus = updatedtimesheet.ApprovalStatus;
            timesheet.DateOfApproval = updatedtimesheet.DateOfApproval;
            timesheet.DateOfSubmission = DateTime.Now;
            timesheet.EditedBy = actionBy;

            var timesheetHistory = timesheet.ToTimesheetRepoModel().ToTimesheetHistoryModel();

            timesheetHistory.Action = "Update";
            timesheetHistory.ActionDate = DateTime.Now;
            timesheetHistory.UserGUID = actionBy;

            await DbContext.TimesheetHistory.AddAsync(timesheetHistory);

            await DbContext.SaveChangesAsync();

            return true; //timesheet updated successfully
        }

        public async Task<bool> UpdateTimesheetActivity(TimesheetActivityRepoModel updatedtimesheetActivity, Guid actionBy)
        {
            var timesheetActivity = await DbContext.TimesheetActivity.FirstOrDefaultAsync(a => a.TimesheetActivityGUID == updatedtimesheetActivity.TimesheetActivityGUID);
            if (timesheetActivity == null)
            {
                return false; //timesheetActivity not found
            }

            timesheetActivity.ActivityGUID = updatedtimesheetActivity.ActivityGUID;
            timesheetActivity.ProjectGUID = updatedtimesheetActivity.ProjectGUID;
            timesheetActivity.TypeOfWork = updatedtimesheetActivity.TypeOfWork;
            timesheetActivity.ActivityDate = updatedtimesheetActivity.ActivityDate;
            timesheetActivity.Hours = updatedtimesheetActivity.Hours;
            timesheetActivity.EditedBy = actionBy;

            var timesheetActivityHistory = timesheetActivity.ToTimesheetActivityRepoModel().ToTimesheetActivityHistoryModel();

            timesheetActivityHistory.Action = "Update";
            timesheetActivityHistory.ActionDate = DateTime.Now;
            timesheetActivityHistory.UserGUID = actionBy;

            await DbContext.TimesheetActivityHistory.AddAsync(timesheetActivityHistory);

            await DbContext.SaveChangesAsync();

            return true; //timesheetActivity updated successfully
        }


        //History
        public async Task<(List<TimesheetHistoryRepoModel> timesheets, int count)> GetTimesheetHistory(Guid timesheetGuid, int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            var take = pageSize;

            var query = DbContext.TimesheetHistory
                .Where(a => a.TimesheetGUID == timesheetGuid)
                .Join(
                    DbContext.Person,
                    timesheetHistory => timesheetHistory.PersonGUID,
                    person => person.PeopleGUID,
                    (timesheetHistory, person) => new { TimesheetHistory = timesheetHistory, Person = person }
                )
                .Join(
                    DbContext.Person,
                    result => result.TimesheetHistory.UserGUID,
                    actionByPerson => actionByPerson.PeopleGUID,
                    (result, actionByPerson) => new { Result = result, ActionByPerson = actionByPerson }
                )
                .Select(result => new TimesheetHistoryRepoModel
                {
                    PersonGUID = result.Result.Person.PeopleGUID,
                    PersonName = $"{result.Result.Person.FirstName} {result.Result.Person.LastName}",
                    TimesheetGUID = result.Result.TimesheetHistory.TimesheetGUID,
                    Month = result.Result.TimesheetHistory.Month,
                    Year = result.Result.TimesheetHistory.Year,
                    DateOfSubmission = result.Result.TimesheetHistory.DateOfSubmission,
                    DateOfApproval = result.Result.TimesheetHistory.DateOfApproval,
                    ApprovalStatus = result.Result.TimesheetHistory.ApprovalStatus,
                    ApprovedBy = result.Result.TimesheetHistory.ApprovedBy,
                    ActionDate = result.Result.TimesheetHistory.ActionDate,
                    Action = result.Result.TimesheetHistory.Action,
                    UserGUID = result.Result.TimesheetHistory.UserGUID,
                    ActionBy = $"{result.ActionByPerson.FirstName} {result.ActionByPerson.LastName}",
                });

            var totalCount = await query.CountAsync();

            query.Skip(skip)
            .Take(take);

            var timesheetHistoryList = await query.ToListAsync();
            return (timesheetHistoryList, totalCount);
        }


        public async Task<List<TimesheetActivityHistoryRepoModel>> GetTimesheetActivityHistory(Guid timesheetGuid, Guid timesheetActivityGuid, int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            var take = pageSize;

            var query = DbContext.TimesheetActivityHistory
            .Where(a => a.TimesheetActivityGUID == timesheetActivityGuid)
            .Skip(skip)
            .Take(take)
            .Join(
                DbContext.Timesheet,
                timesheetActivityHistory => timesheetActivityHistory.TimesheetGUID,
                timesheet => timesheet.TimesheetGUID,
                (timesheetActivityHistory, timesheet) => new { TimesheetActivityHistory = timesheetActivityHistory, Timesheet = timesheet }
            )
            .Join(
                DbContext.Person,
                result => result.Timesheet.PersonGUID,
                person => person.PeopleGUID,
                (result, person) => new { TimesheetActivityHistory = result.TimesheetActivityHistory, Person = person }
            )
            .Join(
                DbContext.Person,
                result => result.TimesheetActivityHistory.UserGUID,
                actionByPerson => actionByPerson.PeopleGUID,
                (result, actionByPerson) => new { TimesheetActivityHistory = result.TimesheetActivityHistory, Person = result.Person, ActionByPerson = actionByPerson }
            )
            .Select(result => new TimesheetActivityHistoryRepoModel
            {
                PersonName = $"{result.Person.FirstName} {result.Person.LastName}",
                TimesheetActivityGUID = result.TimesheetActivityHistory.TimesheetActivityGUID,
                TimesheetGUID = result.TimesheetActivityHistory.TimesheetGUID,
                ProjectGUID = result.TimesheetActivityHistory.ProjectGUID,
                ActivityGUID = result.TimesheetActivityHistory.ActivityGUID,
                TypeOfWork = result.TimesheetActivityHistory.TypeOfWork,
                ActivityDate = result.TimesheetActivityHistory.ActivityDate,
                Hours = result.TimesheetActivityHistory.Hours,
                ActionDate = result.TimesheetActivityHistory.ActionDate,
                Action = result.TimesheetActivityHistory.Action,
                UserGUID = result.TimesheetActivityHistory.UserGUID,
                ActionBy = $"{result.ActionByPerson.FirstName} {result.ActionByPerson.LastName}",
            });

            var timesheetActivityHistoryList = await query.ToListAsync();
            return timesheetActivityHistoryList;
        }


        //Admin
        public async Task<(List<TimesheetRepoModel> timesheets, int totalCount)> GetAllTimesheets(int page, int pageSize, ApprovalStatus status)
        {
            int skip = (page - 1) * pageSize;
            int take = pageSize;
            var query = DbContext.Timesheet
                .Where(a => !a.IsDeleted);
            if (status != ApprovalStatus.All)
            {
                query = query.Where(a => a.ApprovalStatus == status);
            }

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(t => t.Year).ThenBy(t => t.Month);

            var timesheets = await query
                .Join(
                    DbContext.Person,
                    timesheet => timesheet.PersonGUID,
                    person => person.PeopleGUID,
                    (timesheet, person) => new { Timesheet = timesheet, Person = person }
                )
                .Select(result => MapTimesheetToRepoModel(result.Timesheet, result.Person)).Skip(skip).Take(take)
                .ToListAsync();

            return (timesheets, totalCount);
        }
    }
}
