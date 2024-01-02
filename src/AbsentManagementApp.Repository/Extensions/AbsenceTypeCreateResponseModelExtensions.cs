using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsentManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.App.Models;

namespace MainHub.Internal.PeopleAndCulture.App.Repository.Extensions
{
    public static class AbsenceTypeCreateResponseModelExtensions
    {
        public static AbsenceTypeModel ToAbsenceTypeModel(this AbsenceTypeCreateResponseModel model)
        {
            return new AbsenceTypeModel
            {
                //removed AbsenceId/personId because only guid is gonna be used
                Type = model.Type,
                TypeGuid = model.TypeGuid
            };
        }
    }
}
