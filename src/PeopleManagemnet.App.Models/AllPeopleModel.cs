namespace App.Models
{
    public class AllPeopleModel
    {
        public Guid PeopleGUID { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime ExitDate { get; set; }

        public string ChangedBy { get; set; }

        public DateTime ChangeDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string Status { get; set; }
        public bool Selected { get; set; }
        public string Contact { get; set; }
        public string EmergencyContact { get; set; }
        public AllPeopleModel()
        {
            PeopleGUID = Guid.NewGuid();
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            BirthDate = DateTime.MinValue;
            EntryDate = DateTime.MinValue;
            ExitDate = DateTime.MinValue;
            ChangedBy = string.Empty;
            ChangeDate = DateTime.MinValue;
            CreatedBy = string.Empty;
            CreatedDate = DateTime.MinValue;
            Status = string.Empty;
            Contact = string.Empty;
            EmergencyContact = string.Empty;
        }
    }
}
