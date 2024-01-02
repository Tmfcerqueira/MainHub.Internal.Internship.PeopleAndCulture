using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.Api.Models
{
    public class ApiPartnerResponseModel
    {
        public Guid PartnerGUID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Locality { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string TaxNumber { get; set; }
        public string ServiceDescription { get; set; }
        public string Observation { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ChangedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string State { get; set; }
        public bool IsDeleted { get; set; }
        public Guid DeletedBy { get; set; }
        public ApiPartnerResponseModel()
        {
            PartnerGUID = Guid.Empty;
            Name = string.Empty;
            PhoneNumber = string.Empty;
            Address = string.Empty;
            Locality = string.Empty;
            PostalCode = string.Empty;
            Country = string.Empty;
            TaxNumber = string.Empty;
            ServiceDescription = string.Empty;
            Observation = string.Empty;
            CreationDate = new DateTime(2999, 12, 31, 23, 59, 54);
            CreatedBy = "System";
            ChangedDate = new DateTime(2999, 12, 31, 23, 59, 54);
            ModifiedBy = "None";
            State = "Active";
            IsDeleted = false;
            DeletedBy = Guid.Empty;
        }
    }
}
