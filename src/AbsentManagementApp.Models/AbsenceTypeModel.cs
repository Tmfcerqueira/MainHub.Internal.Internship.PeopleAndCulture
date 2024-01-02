using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.App.Models
{
    public class AbsenceTypeModel
    {
        public int Id { get; set; }
        public Guid TypeGuid { get; set; }
        public string Type { get; set; }

        public AbsenceTypeModel()
        {
            Id = 0;
            TypeGuid = Guid.Empty;
            Type = "Other";
        }
    }
}
