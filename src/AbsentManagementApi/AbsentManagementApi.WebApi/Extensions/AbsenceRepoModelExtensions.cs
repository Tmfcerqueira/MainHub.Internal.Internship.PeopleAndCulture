using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Extensions
{
    public static class AbsenceRepoModelExtensions
    {
        public static AbsenceResponseModel ToAbsenceResponseModel(this AbsenceRepoModel model)
        {
            return new AbsenceResponseModel
            {
                //removed Id because only guid is gonna be used
                AbsenceEnd = model.AbsenceEnd,
                AbsenceStart = model.AbsenceStart,
                AbsenceTypeGuid = model.AbsenceTypeGuid,
                PersonName = model.PersonName,
                PersonGuid = model.PersonGuid,
                Schedule = model.Schedule,
                ApprovalDate = model.ApprovalDate,
                SubmissionDate = model.SubmissionDate,
                AbsenceGuid = model.AbsenceGuid,
                ApprovalStatus = model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                Description = model.Description
            };
        }
    }
}
