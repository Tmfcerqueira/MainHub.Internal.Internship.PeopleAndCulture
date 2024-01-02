using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsentManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.App.Models;
using MainHub.Internal.PeopleAndCulture.App.Repository;
using MainHub.Internal.PeopleAndCulture.Common;
using ApprovalStatus = AbsentManagement.Api.Proxy.Client.Model.ApprovalStatus;

namespace MainHub.Internal.PeopleAndCulture.App.Repository.Extensions
{
    public static class AbsenceCreateResponseModelExtensions
    {
        public static AbsenceModel ToAbsenceModel(this AbsenceCreateResponseModel model)
        {
            if (!model.ApprovalStatus.HasValue)
            {
                model.ApprovalStatus = ApprovalStatus.Draft;
            }
            return new AbsenceModel
            {
                AbsenceGuid = model.AbsenceGuid,
                PersonGuid = model.PersonGuid,
                AbsenceEnd = model.AbsenceEnd,
                AbsenceStart = model.AbsenceStart,
                AbsenceTypeGuid = model.AbsenceTypeGuid,
                Schedule = model.Schedule,
                ApprovalDate = model.ApprovalDate,
                ApprovedBy = model.ApprovedBy,
                SubmissionDate = model.SubmissionDate,
                ApprovalStatus = (Common.ApprovalStatus)model.ApprovalStatus,
                Description = model.Description
            };
        }
    }
}
