using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Extensions
{
    public static class AbsenceDbModelExtensions
    {

        public static AbsenceRepoModel ToAbsenceRepoModel(this Absence model)
        {
            return new AbsenceRepoModel
            {
                //removed AbsenceId/personId because only guid is gonna be used
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
                CreatedBy = model.CreatedBy,
                ModifiedBy = model.ModifiedBy,
                DeletedBy = model.DeletedBy,
                Description = model.Description
            };
        }

    }
}
