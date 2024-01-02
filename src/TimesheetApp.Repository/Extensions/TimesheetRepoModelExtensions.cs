using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;
using TimesheetManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class TimesheetRepoModelExtensions
    {
        public static TimesheetModel ToTimesheetModel(this TimesheetResponseModel model)
        {
            if (!model.ApprovalStatus.HasValue)
            {
                model.ApprovalStatus = (TimesheetManagement.Api.Proxy.Client.Model.ApprovalStatus?)Common.ApprovalStatus.Draft;
            }

            return new TimesheetModel
            {
                TimesheetGUID = model.TimesheetGUID,
                PersonGUID = model.PersonGUID,
                PersonName = model.PersonName,
                Month = model.Month,
                Year = model.Year,
                ApprovalStatus = (Common.ApprovalStatus)model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                DateOfSubmission = model.DateOfSubmission,
                DateOfApproval = model.DateOfApproval
            };
        }
    }
}
