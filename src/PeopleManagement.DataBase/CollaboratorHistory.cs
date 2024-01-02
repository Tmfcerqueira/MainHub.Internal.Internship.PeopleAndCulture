using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleManagementDataBase;

namespace MainHub.Internal.PeopleAndCulture
{
    public class CollaboratorHistory : CollaboratorBase
    {
        public int Id { get; set; }
        public string Action { get; set; }

        public DateTime ActionDate { get; set; }
        public string UserID { get; set; }
    }
}
