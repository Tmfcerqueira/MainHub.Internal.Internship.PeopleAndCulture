using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsentManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.App.Models;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class AbsenceHistoryResponseModelExtensions
    {
        public static AbsenceHistoryModel ToAbsenceHistoryModel(this AbsenceHistoryResponseModel model)
        {

            if (!model.ApprovalStatus.HasValue)
            {
                model.ApprovalStatus = ApprovalStatus.Draft;
            }
            return new AbsenceHistoryModel
            {
                //removed AbsenceId/personId because only guid is gonna be used
                PersonName = model.PersonName,
                PersonGuid = model.PersonGuid,
                AbsenceEnd = model.AbsenceEnd,
                AbsenceStart = model.AbsenceStart,
                AbsenceTypeGuid = model.AbsenceTypeGuid,
                Schedule = model.Schedule,
                ApprovalDate = model.ApprovalDate,
                AbsenceGuid = model.AbsenceGuid,
                ApprovalStatus = (Common.ApprovalStatus)model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                SubmissionDate = model.SubmissionDate,
                ActionDate = model.ActionDate,
                ActionText = model.ActionText,
                UserId = model.UserId,
                ActionBy = model.ActionBy,
                Description = model.Description
            };
        }
    }
}
