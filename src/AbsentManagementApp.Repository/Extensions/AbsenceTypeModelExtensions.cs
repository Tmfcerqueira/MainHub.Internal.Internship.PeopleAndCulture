using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsentManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.App.Models;

namespace MainHub.Internal.PeopleAndCulture.App.Repository.Extensions
{
    public static class AbsenceTypeModelExtensions
    {
        public static AbsenceTypeCreateRequestModel ToAbsenceTypeCreateRequestModel(this AbsenceTypeModel model)
        {
            return new AbsenceTypeCreateRequestModel
            {
                Type = model.Type
            };
        }
    }
}
