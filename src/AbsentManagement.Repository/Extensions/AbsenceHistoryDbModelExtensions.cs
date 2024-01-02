using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Extensions
{
    internal static class AbsenceHistoryDbModelExtensions
    {

        public static AbsenceHistoryRepoModel ToAbsenceHistoryRepoModel(this AbsenceHistory model)
        {
            return new AbsenceHistoryRepoModel
            {
                //removed AbsenceId because only guid is gonna be used
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
