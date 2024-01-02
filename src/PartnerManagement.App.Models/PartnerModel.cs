using System.ComponentModel.DataAnnotations;

namespace PartnerManagement.App.Models
{
    public class PartnerModel
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
        [Required]
        public bool IsDeleted { get; set; }
        public Guid DeletedBy { get; set; }
        public PartnerModel()
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
            ModifiedBy = "System";
            State = "Active";
            IsDeleted = false;
            DeletedBy = Guid.Empty;
        }
    }
}
