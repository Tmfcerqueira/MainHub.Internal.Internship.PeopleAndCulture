using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models
{
    public class AbsenceTypeCreateRequestModel
    {
        public string Type { get; set; }

        public AbsenceTypeCreateRequestModel(string type)
        {
            Type = type;
        }

        public AbsenceTypeCreateRequestModel()
        {
            Type = "Has no Type";
        }
    }
}
