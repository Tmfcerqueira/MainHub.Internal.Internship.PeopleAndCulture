namespace MainHub.Internal.PeopleAndCulture
{
    public class PeopleHistory
    {
        public Guid PeopleGuid { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }

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

        public string Action { get; set; }

        public DateTime ActionDate { get; set; }
        public string UserID { get; set; }

        public string Iban { get; set; }

        public Contract ContractType { get; set; }
        public string Observations { get; set; }
        public int Employee_Id { get; set; }
        public string Contact { get; set; }
        public string EmergencyContact { get; set; }

        public PeopleHistory()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Adress = string.Empty;
            Postal = string.Empty;
            Locality = string.Empty;
            Country = string.Empty;
            TaxNumber = string.Empty;
            CCNumber = string.Empty;
            SSNumber = string.Empty;
            CivilState = string.Empty;
            ChangedBy = string.Empty;
            CreatedBy = string.Empty;
            Status = string.Empty;
            DeletedBy = string.Empty;
            Action = string.Empty;
            UserID = string.Empty;
            Iban = string.Empty;
            Observations = string.Empty;
            Contact = string.Empty;
            EmergencyContact = string.Empty;
            ContractType = Contract.NoTerm;
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
}
