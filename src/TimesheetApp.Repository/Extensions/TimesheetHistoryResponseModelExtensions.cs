using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimesheetManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class TimesheetHistoryResponseModelExtensions
    {
        public static TimesheetHistoryModel ToTimesheetHistoryModel(this TimesheetHistoryResponseModel model)
        {

            if (!model.ApprovalStatus.HasValue)
            {
                model.ApprovalStatus = ApprovalStatus.Draft;
            }
            return new TimesheetHistoryModel
            {
                TimesheetGUID = model.TimesheetGUID,
                PersonGUID = model.PersonGUID,
                PersonName = model.PersonName,
                Month = model.Month,
                Year = model.Year,
                ApprovalStatus = (Common.ApprovalStatus)model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                DateOfSubmission = model.DateOfSubmission,
                DateOfApproval = model.DateOfApproval,
                Action = model.Action,
                ActionDate = model.ActionDate,
                ActionBy = model.ActionBy,
                UserGUID = model.UserGUID,
            };
        }
    }
}
