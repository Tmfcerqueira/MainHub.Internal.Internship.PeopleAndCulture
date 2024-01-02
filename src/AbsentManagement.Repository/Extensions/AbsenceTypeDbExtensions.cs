using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Extensions
{
    internal static class AbsenceTypeDbExtensions
    {
        public static AbsenceType ToAbsenceTypeDbModel(this AbsenceTypeRepoModel model)
        {
            return new AbsenceType
            {
                Type = model.Type,
                TypeGuid = model.TypeGuid
            };
        }

        public static AbsenceTypeHistory ToAbsenceTypeHistoryModel(this AbsenceTypeRepoModel model)
        {
            return new AbsenceTypeHistory
            {
                Type = model.Type,
                TypeGuid = model.TypeGuid
            };
        }
    }
}
