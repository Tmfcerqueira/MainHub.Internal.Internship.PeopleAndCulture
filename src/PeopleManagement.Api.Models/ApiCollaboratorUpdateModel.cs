using static PeopleManagement.Api.Models.ApiCollaboratorResponseModel;

namespace PeopleManagement.Api.Models
{
    public class ApiCollaboratorUpdateModel
    {
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

        public DateTime ChangeDate { get; set; }

        public string ChangedBy { get; set; }

        public string Status { get; set; }

        public string Iban { get; set; }
        public Contract ContractType { get; set; }
        public string Observations { get; set; }
        public int Employee_Id { get; set; }
        public string Contact { get; set; }
        public string EmergencyContact { get; set; }

        public ApiCollaboratorUpdateModel()
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
            Status = string.Empty;
            ChangeDate = DateTime.Now;
            Iban = string.Empty;
            Observations = string.Empty;
            ContractType = Contract.NoTerm;
            Contact = string.Empty;
            EmergencyContact = string.Empty;
        }
    }
}
