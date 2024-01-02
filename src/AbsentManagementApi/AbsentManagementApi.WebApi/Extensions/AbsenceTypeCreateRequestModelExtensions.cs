using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Extensions
{
    public static class AbsenceTypeCreateRequestModelExtensions
    {
        public static AbsenceTypeRepoModel ToAbsenceTypeRepoModel(this AbsenceTypeCreateRequestModel model)
        {
            return new AbsenceTypeRepoModel
            {
                Type = model.Type,
                TypeGuid = Guid.Empty
            };
        }
    }
}
