using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartnerManagement.App.Models
{
    public class ContactModel
    {
        public Guid ContactGUID { get; set; }
        public Guid PartnerGUID { get; set; } // Supposed to be the Foreign Key
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public string Observation { get; set; }
        public bool IsDeleted { get; set; }
        public Guid DeletedBy { get; set; }
        public Guid UserGUID { get; set; }
        public ContactModel()
        {
            ContactGUID = Guid.Empty;
            PartnerGUID = Guid.Empty;
            Name = string.Empty;
            Email = string.Empty;
            Role = string.Empty;
            PhoneNumber = string.Empty;
            Department = string.Empty;
            Observation = string.Empty;
            IsDeleted = false;
            DeletedBy = Guid.Empty;
            UserGUID = Guid.Empty;
        }
    }
}
