using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models
{
    public class AbsenceTypeRepoModel
    {
        public Guid TypeGuid { get; set; }
        public string Type { get; set; }

        public Guid CreatedBy { get; set; }

        public AbsenceTypeRepoModel()
        {
            TypeGuid = Guid.Empty;
            Type = "Has no Type";
            CreatedBy = Guid.Empty;
        }
    }
}
