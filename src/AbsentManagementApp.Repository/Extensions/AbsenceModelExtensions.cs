using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsentManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.App.Models;

namespace MainHub.Internal.PeopleAndCulture.App.Repository.Extensions
{
    public static class AbsenceModelExtensions
    {
        public static AbsenceCreateRequestModel ToAbsenceCreateRequestModel(this AbsenceModel model)
        {
            return new AbsenceCreateRequestModel
            {
                //removed AbsenceId because only guid is gonna be used
                PersonGuid = model.PersonGuid,
                AbsenceEnd = model.AbsenceEnd,
                AbsenceStart = model.AbsenceStart,
                AbsenceTypeGuid = model.AbsenceTypeGuid,
                Schedule = model.Schedule,
                ApprovalStatus = (ApprovalStatus?)model.ApprovalStatus,
                SubmissionDate = model.SubmissionDate,
                Description = model.Description
            };
        }

        public static AbsenceUpdateModel ToAbsenceUpdateModel(this AbsenceModel model)
        {
            return new AbsenceUpdateModel
            {
                //removed AbsenceId because only guid is gonna be used
                PersonGuid = model.PersonGuid,
                AbsenceGuid = model.AbsenceGuid,
                AbsenceEnd = model.AbsenceEnd,
                AbsenceStart = model.AbsenceStart,
                AbsenceTypeGuid = model.AbsenceTypeGuid,
                ApprovalDate = model.ApprovalDate,
                ApprovalStatus = (ApprovalStatus?)model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                SubmissionDate = model.SubmissionDate,
                Schedule = model.Schedule,
                Description = model.Description
            };
        }
    }
}
