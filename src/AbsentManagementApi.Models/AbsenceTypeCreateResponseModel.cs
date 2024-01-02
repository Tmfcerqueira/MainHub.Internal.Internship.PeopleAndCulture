using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models
{
    public class AbsenceTypeCreateResponseModel
    {
        public Guid TypeGuid { get; set; }
        public string Type { get; set; }

        public AbsenceTypeCreateResponseModel()
        {
            TypeGuid = Guid.Empty;
            Type = "Has no Type";
        }

        public AbsenceTypeCreateResponseModel(Guid typeGuid, string type)
        {
            TypeGuid = typeGuid;
            Type = type;
        }
    }
}
