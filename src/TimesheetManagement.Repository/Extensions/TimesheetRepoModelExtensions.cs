using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;
using TimesheetManagement.DataBase.Models;

namespace TimesheetManagement.Repository.Extensions
{
    public static class TimesheetRepoModelExtensions
    {
        public static TimesheetRepoModel ToTimesheetRepoModel(this Timesheet model)
        {
            return new TimesheetRepoModel
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
                IsDeleted = model.IsDeleted
            };
        }
    }
}
