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
    internal static class AbsenceTypeRepoModelExtensions
    {
        public static AbsenceTypeRepoModel ToAbsenceTypeRepoModel(this AbsenceType model)
        {
            return new AbsenceTypeRepoModel
            {
                Type = model.Type,
                TypeGuid = model.TypeGuid,
                CreatedBy = model.CreatedBy
            };
        }
    }
}
