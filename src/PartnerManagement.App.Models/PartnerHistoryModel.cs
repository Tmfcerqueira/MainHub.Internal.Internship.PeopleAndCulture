using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartnerManagement.App.Models
{
    public class PartnerHistoryModel
    {
        public Guid PartnerGUID { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Locality { get; set; }
        [Required]
        [MinLength(5)]
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [Range(13, 13)]
        public string TaxNumber { get; set; }
        [Required]
        public string ServiceDescription { get; set; }
        public string Observation { get; set; }
        public DateTime CreationDate { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        public DateTime ChangedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string State { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        [Required]
        public Guid UserGUID { get; set; }
        public PartnerHistoryModel()
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
            CreatedBy = string.Empty;
            ChangedDate = new DateTime(2999, 12, 31, 23, 59, 54);
            ModifiedBy = string.Empty;
            State = "Active";
            Action = string.Empty;
            ActionDate = new DateTime(2999, 12, 31, 23, 59, 54);
            UserGUID = Guid.Empty;
        }
    }
}
