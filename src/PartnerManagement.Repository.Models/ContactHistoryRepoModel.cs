using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture
{
    public class ContactHistoryRepoModel
    {
        public Guid ContactGUID { get; set; }
        public Guid PartnerGUID { get; set; } // Supposed to be the Foreign Key
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public string Observation { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        public Guid UserGUID { get; set; }
        public ContactHistoryRepoModel()
        {
            ContactGUID = Guid.Empty;
            PartnerGUID = Guid.Empty;
            Name = string.Empty;
            Email = string.Empty;
            Role = string.Empty;
            PhoneNumber = string.Empty;
            Department = string.Empty;
            Observation = string.Empty;
            Action = string.Empty;
            ActionDate = new DateTime(2999, 12, 31, 23, 59, 54);
            UserGUID = Guid.Empty;
        }
    }
}
