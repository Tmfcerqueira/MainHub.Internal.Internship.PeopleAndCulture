namespace PeopleManagementRepository.Models
{
    public class AllPeopleRepoModel
    {
        public Guid PeopleGUID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime ExitDate { get; set; }
        public string Status { get; set; }
        public string Contact { get; set; }
        public string EmergencyContact { get; set; }
        public AllPeopleRepoModel()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Status = string.Empty;
            Contact = string.Empty;
            EmergencyContact = string.Empty;
        }
    }
}
