using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;
using MainHub.Internal.PeopleAndCulture.Common;



namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Extensions
{
    public static class AbsenceCreateRequestModelExtensions
    {

        public static AbsenceRepoModel ToAbsenceRepoModel(this AbsenceCreateRequestModel model)
        {
            return new AbsenceRepoModel
            {
                //removed Id because only guid is gonna be used
                AbsenceEnd = model.AbsenceEnd,
                AbsenceStart = model.AbsenceStart,
                AbsenceTypeGuid = model.AbsenceTypeGuid,
                PersonGuid = model.PersonGuid,
                Schedule = model.Schedule,
                ApprovalStatus = model.ApprovalStatus,
                SubmissionDate = model.SubmissionDate,
                Description = model.Description
            };
        }

    }
}
