using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsentManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.App.Models;

namespace MainHub.Internal.PeopleAndCulture.App.Repository.Extensions
{
    public static class AbsenceTypeResponseModelExtensions
    {
        public static AbsenceTypeModel ToAbsenceTypeModel(this AbsenceTypeResponseModel model)
        {
            //removed AbsenceTypeId because only guid is gonna be used
            return new AbsenceTypeModel
            {
                Type = model.Type,
                TypeGuid = model.TypeGuid
            };
        }
    }
}
