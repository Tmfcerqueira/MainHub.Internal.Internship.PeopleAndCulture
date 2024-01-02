using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Extensions
{
    public static class AbsenceCreateResponseModelExtensions
    {
        public static AbsenceCreateResponseModel ToAbsenceCreateResponseModel(this AbsenceRepoModel model)
        {
            return new AbsenceCreateResponseModel
            {
                AbsenceEnd = model.AbsenceEnd,
                AbsenceStart = model.AbsenceStart,
                AbsenceTypeGuid = model.AbsenceTypeGuid,
                PersonGuid = model.PersonGuid,
                AbsenceGuid = model.AbsenceGuid,
                ApprovalDate = model.ApprovalDate,
                ApprovalStatus = model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                SubmissionDate = model.SubmissionDate,
                Schedule = model.Schedule,
                Description = model.Description
            };
        }
    }
}
