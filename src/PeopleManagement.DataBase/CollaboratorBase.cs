using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture
{
    public class CollaboratorBase
    {
        public Guid PeopleGUID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Adress { get; set; }

        public string Postal { get; set; }
        public string Locality { get; set; }

        public string Country { get; set; }

        public string TaxNumber { get; set; }

        public string CCNumber { get; set; }

        public string SSNumber { get; set; }

        public DateTime CCVal { get; set; }

        public string CivilState { get; set; }

        public int DependentNum { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime ExitDate { get; set; }

        public DateTime CreationDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ChangeDate { get; set; }

        public string ChangedBy { get; set; }

        public string Status { get; set; }

        public string DeletedBy { get; set; }

        public DateTime DeletedDate { get; set; }

        public bool IsDeleted { get; set; }

        public string Email { get; set; }

        public string Iban { get; set; }
        public Contract ContractType { get; set; }

        public string Observations { get; set; }
        public int Employee_Id { get; set; }
        public string Contact { get; set; }
        public string EmergencyContact { get; set; }
    }
    public enum Contract
    {
        Certo,
        Incerto,
        NoTerm,
        Curta,
        Parcial,
    }
}
