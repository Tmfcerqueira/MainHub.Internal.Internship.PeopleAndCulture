using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Extensions
{
    public static class AbsenceHistoryRepoModelExtensions
    {
        public static AbsenceHistoryResponseModel ToAbsenceHistoryResponseModel(this AbsenceHistoryRepoModel model)
        {
            return new AbsenceHistoryResponseModel
            {
                //removed Id because only guid is gonna be used
                PersonName = model.PersonName,
                AbsenceEnd = model.AbsenceEnd,
                AbsenceStart = model.AbsenceStart,
                AbsenceTypeGuid = model.AbsenceTypeGuid,
                PersonGuid = model.PersonGuid,
                Schedule = model.Schedule,
                ApprovalDate = model.ApprovalDate,
                SubmissionDate = model.SubmissionDate,
                AbsenceGuid = model.AbsenceGuid,
                ApprovalStatus = model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                ActionDate = model.ActionDate,
                ActionText = model.ActionText,
                UserId = model.UserId,
                ActionBy = model.ActionBy,
                Description = model.Description
            };
        }
    }
}
