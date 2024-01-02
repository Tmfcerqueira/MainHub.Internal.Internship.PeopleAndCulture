

using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Extensions
{
    public static class AbsenceTypeRepoModelExtensions
    {
        public static AbsenceTypeResponseModel ToAbsenceTypeResponseModel(this AbsenceTypeRepoModel model)
        {
            return new AbsenceTypeResponseModel
            {
                Type = model.Type,
                TypeGuid = model.TypeGuid
            };
        }
    }
}
