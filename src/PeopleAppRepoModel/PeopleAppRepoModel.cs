namespace App.Repository
{
    public class PeopleAppRepoModel
    {
        public Guid PeopleGUID { get; set; }

        public string? Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string? Adress { get; set; }

        public string? Postal { get; set; }

        public string? Country { get; set; }

        public string? TaxNumber { get; set; }

        public string? CCNumber { get; set; }

        public string? SSNumber { get; set; }

        public DateTime CCVal { get; set; }

        public string? CivilState { get; set; }

        public int DependentNum { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime ExitDate { get; set; }

        public DateTime CreationDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime ChangeDate { get; set; }

        public string? ChangedBy { get; set; }

        public string? Status { get; set; }
    }
}
