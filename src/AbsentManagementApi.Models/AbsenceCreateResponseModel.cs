using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MainHub.Internal.PeopleAndCulture.Common;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models
{
    public class AbsenceCreateResponseModel
    {

        public Guid AbsenceGuid { get; set; }
        public Guid PersonGuid { get; set; }
        public Guid AbsenceTypeGuid { get; set; }
        public DateTime AbsenceStart { get; set; }
        public DateTime AbsenceEnd { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime SubmissionDate { get; set; }

        public string Schedule { get; set; }

        public string Description { get; set; }

        public AbsenceCreateResponseModel()
        {
            AbsenceGuid = Guid.Empty;
            PersonGuid = Guid.Empty;
            AbsenceTypeGuid = Guid.Empty;
            AbsenceStart = DateTime.Now;
            AbsenceEnd = DateTime.Now;
            ApprovalStatus = ApprovalStatus.Draft;
            ApprovedBy = "None";
            ApprovalDate = new DateTime(2999, 12, 31, 23, 59, 59);
            SubmissionDate = new DateTime(2999, 12, 31, 23, 59, 59);
            Schedule = "Full Day";
            Description = "None";
        }

    }
}
