using MainHub.Internal.PeopleAndCulture.Common;

namespace MainHub.Internal.PeopleAndCulture.App.Models
{
    public class AbsenceHistoryModel
    {
        public Guid AbsenceGuid { get; set; }
        public string PersonName { get; set; }
        public Guid PersonGuid { get; set; }
        public Guid AbsenceTypeGuid { get; set; }
        public DateTime AbsenceStart { get; set; }
        public DateTime AbsenceEnd { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Schedule { get; set; }
        public string AbsenceType { get; set; }
        public string ActionText { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionBy { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }

        //ctor with no params
        public AbsenceHistoryModel()
        {
            PersonName = "None";
            AbsenceStart = DateTime.Now;
            AbsenceEnd = DateTime.Now;
            AbsenceGuid = Guid.Empty;
            PersonGuid = Guid.Empty;
            ApprovedBy = "None";
            AbsenceTypeGuid = Guid.Empty;
            ApprovalStatus = ApprovalStatus.Draft;
            ApprovalDate = new DateTime(2999, 12, 31, 23, 59, 59);
            SubmissionDate = DateTime.Now;
            Schedule = "Full Day";
            AbsenceType = "Other";
            ActionText = "None";
            ActionDate = DateTime.Now;
            UserId = Guid.Empty;
            ActionBy = "None";
            Description = "None";
        }

    }
}

