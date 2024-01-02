using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Extensions
{
    public static class AbsenceHistoryRepoModelExtensions
    {
        public static AbsenceHistory ToAbsenceHistoryDbModel(this AbsenceHistoryRepoModel model)
        {
            return new AbsenceHistory
            {
                PersonGuid = model.PersonGuid,
                AbsenceGuid = model.AbsenceGuid,
                AbsenceEnd = model.AbsenceEnd,
                AbsenceStart = model.AbsenceStart,
                AbsenceTypeGuid = model.AbsenceTypeGuid,
                ApprovalDate = model.ApprovalDate,
                ApprovalStatus = model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                SubmissionDate = model.SubmissionDate,
                Schedule = model.Schedule,
                ActionDate = model.ActionDate,
                ActionText = model.ActionText,
                UserId = model.UserId,
                Description = model.Description
            };
        }

    }
}
