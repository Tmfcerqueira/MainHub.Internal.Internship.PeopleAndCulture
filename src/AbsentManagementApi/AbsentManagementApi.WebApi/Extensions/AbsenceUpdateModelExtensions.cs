using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;
using Microsoft.IdentityModel.Abstractions;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Extensions
{
    public static class AbsenceUpdateModelExtensions
    {
        public static AbsenceRepoModel ToAbsenceRepoModel(this AbsenceUpdateModel model)
        {
            return new AbsenceRepoModel()
            {
                AbsenceEnd = model.AbsenceEnd,
                AbsenceGuid = model.AbsenceGuid,
                AbsenceTypeGuid = model.AbsenceTypeGuid,
                AbsenceStart = model.AbsenceStart,
                Schedule = model.Schedule,
                ApprovalDate = model.ApprovalDate,
                SubmissionDate = model.SubmissionDate,
                ApprovalStatus = model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                PersonGuid = model.PersonGuid,
                Description = model.Description
            };
        }
    }
}
