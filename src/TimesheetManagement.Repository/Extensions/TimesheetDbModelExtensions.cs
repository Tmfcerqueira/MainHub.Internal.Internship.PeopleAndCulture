using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Models;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;
using TimesheetManagement.DataBase.Models;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    internal static class TimesheetDbModelExtensions
    {
        public static Timesheet ToTimesheetDbModel(this TimesheetRepoModel model)
        {
            return new Timesheet
            {
                TimesheetGUID = model.TimesheetGUID,
                PersonGUID = model.PersonGUID,
                Month = model.Month,
                Year = model.Year,
                ApprovalStatus = model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                DateOfSubmission = model.DateOfSubmission,
                DateOfApproval = model.DateOfApproval,
                CreatedBy = model.CreatedBy,
                EditedBy = model.EditedBy,
                DeletedBy = model.DeletedBy,
            };
        }

        public static TimesheetHistory ToTimesheetHistoryModel(this TimesheetRepoModel model)
        {
            return new TimesheetHistory
            {
                TimesheetGUID = model.TimesheetGUID,
                PersonGUID = model.PersonGUID,
                Month = model.Month,
                Year = model.Year,
                ApprovalStatus = model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                DateOfSubmission = model.DateOfSubmission,
                DateOfApproval = model.DateOfApproval,
            };
        }
    }
}
