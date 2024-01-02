using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models
{
    public class AbsenceTypeResponseModel
    {
        public Guid TypeGuid { get; set; }
        public string Type { get; set; }

        public AbsenceTypeResponseModel()
        {
            TypeGuid = Guid.Empty;
            Type = "Has no Type";
        }
    }
}
