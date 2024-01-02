using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartnerManagement.Api.Models.API_Partners
{
    public class ApiPartnerCreateRequestModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Locality { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string TaxNumber { get; set; }
        public string ServiceDescription { get; set; }
        public string Observation { get; set; }
        public string CreatedBy { get; set; }
        public ApiPartnerCreateRequestModel()
        {
            Name = string.Empty;
            PhoneNumber = string.Empty;
            Address = string.Empty;
            Locality = string.Empty;
            PostalCode = string.Empty;
            Country = string.Empty;
            TaxNumber = string.Empty;
            ServiceDescription = string.Empty;
            Observation = string.Empty;
            CreatedBy = string.Empty;
        }
    }
}
