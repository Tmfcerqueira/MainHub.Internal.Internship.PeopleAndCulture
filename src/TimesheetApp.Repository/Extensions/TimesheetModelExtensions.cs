using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;
using TimesheetManagement.Api.Proxy.Client.Model;
using Proxy = TimesheetManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class TimesheetModelExtensions
    {
        public static TimesheetCreateRequestModel ToTimesheetCreateRequestModel(this TimesheetModel model)
        {
            return new TimesheetCreateRequestModel
            {
                PersonGUID = model.PersonGUID,
                Month = model.Month,
                Year = model.Year
            };
        }
        public static TimesheetModel ToTimesheetResponseModel(this TimesheetCreateResponseModel responseModel)
        {
            if (!responseModel.ApprovalStatus.HasValue)
            {
                responseModel.ApprovalStatus = Proxy.ApprovalStatus.Draft;
            }
            return new TimesheetModel
            {
                TimesheetGUID = responseModel.TimesheetGUID,
                PersonGUID = responseModel.PersonGUID,
                Month = responseModel.Month,
                Year = responseModel.Year,
                ApprovalStatus = (Common.ApprovalStatus)responseModel.ApprovalStatus,
                ApprovedBy = responseModel.ApprovedBy,
                DateOfSubmission = responseModel.DateOfSubmission,
                DateOfApproval = responseModel.DateOfApproval
            };
        }

        public static TimesheetUpdateModel ToTimesheetUpdateModel(this TimesheetModel model)
        {
            return new TimesheetUpdateModel
            {
                TimesheetGUID = model.TimesheetGUID,
                PersonGUID = model.PersonGUID,
                Month = model.Month,
                Year = model.Year,
                ApprovalStatus = (Proxy.ApprovalStatus?)model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                DateOfSubmission = model.DateOfSubmission,
                DateOfApproval = model.DateOfApproval
            };
        }
    }
}
