using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models
{
    public class AbsenceTypeHistory
    {
        public int Id { get; set; }
        public Guid TypeGuid { get; set; }
        public string Type { get; set; }

        public string ActionText { get; set; }

        public DateTime ActionDate { get; set; }

        public Guid UserId { get; set; }



        //ctor with no params

        public AbsenceTypeHistory()
        {
            Id = 0;
            TypeGuid = Guid.Empty;
            Type = "Has no Type";
            ActionText = "None";
            ActionDate = DateTime.Now;
            UserId = Guid.Empty;
        }
    }
}
