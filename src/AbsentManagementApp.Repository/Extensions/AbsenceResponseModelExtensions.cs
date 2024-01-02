using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsentManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.App.Models;

namespace MainHub.Internal.PeopleAndCulture.App.Repository.Extensions
{
    public static class AbsenceResponseModelExtensions
    {
        public static AbsenceModel ToAbsenceModel(this AbsenceResponseModel model)
        {

            if (!model.ApprovalStatus.HasValue)
            {
                model.ApprovalStatus = ApprovalStatus.Draft;
            }
            return new AbsenceModel
            {
                //removed AbsenceIdbecause only guid is gonna be used
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
                Description = model.Description
            };
        }

        public static AbsenceUpdateModel ToAbsenceUpdateModel(this AbsenceResponseModel model)
        {

            if (!model.ApprovalStatus.HasValue)
            {
                model.ApprovalStatus = ApprovalStatus.Draft;
            }
            return new AbsenceUpdateModel
            {
                //removed AbsenceId/personId because only guid is gonna be used
                PersonGuid = model.PersonGuid,
                AbsenceEnd = model.AbsenceEnd,
                AbsenceStart = model.AbsenceStart,
                AbsenceTypeGuid = model.AbsenceTypeGuid,
                Schedule = model.Schedule,
                ApprovalDate = model.ApprovalDate,
                AbsenceGuid = model.AbsenceGuid,
                ApprovalStatus = model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                SubmissionDate = model.SubmissionDate,
                Description = model.Description
            };
        }

    }
}
