using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models
{
    public class AbsenceUpdateModel
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

        //ctor with no params
        public AbsenceUpdateModel()
        {
            AbsenceEnd = new DateTime(2999, 12, 31, 23, 59, 59);
            AbsenceStart = new DateTime(2999, 12, 31, 23, 59, 59);
            AbsenceGuid = Guid.Empty;
            PersonGuid = Guid.Empty;
            ApprovedBy = "None";
            AbsenceTypeGuid = Guid.Empty;
            ApprovalStatus = ApprovalStatus.Draft;
            ApprovalDate = new DateTime(2999, 12, 31, 23, 59, 59);
            SubmissionDate = new DateTime(2999, 12, 31, 23, 59, 59);
            Schedule = "FullDay";
            Description = "None";
        }
    }
}
