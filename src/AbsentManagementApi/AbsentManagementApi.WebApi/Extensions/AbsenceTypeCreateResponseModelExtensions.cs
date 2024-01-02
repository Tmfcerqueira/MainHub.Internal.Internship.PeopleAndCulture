using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Extensions
{
    public static class AbsenceTypeCreateResponseModelExtensions
    {

        public static AbsenceTypeCreateResponseModel ToAbsenceTypeCreateResponseModel(this AbsenceTypeRepoModel model)
        {
            return new AbsenceTypeCreateResponseModel
            {
                Type = model.Type,
                TypeGuid = model.TypeGuid
            };
        }
    }
}
