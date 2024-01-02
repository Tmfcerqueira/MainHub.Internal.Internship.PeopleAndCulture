using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models
{
    public class AbsenceCreateRequestModel
    {

        public Guid PersonGuid { get; set; }
        public Guid AbsenceTypeGuid { get; set; }
        public DateTime AbsenceStart { get; set; }
        public DateTime AbsenceEnd { get; set; }
        public string Schedule { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Description { get; set; }


        public AbsenceCreateRequestModel()
        {
            PersonGuid = Guid.Empty;
            AbsenceTypeGuid = Guid.Empty;
            AbsenceStart = DateTime.Now;
            AbsenceEnd = DateTime.Now;
            ApprovalStatus = ApprovalStatus.Draft;
            SubmissionDate = new DateTime(2999, 12, 31, 23, 59, 59);
            Schedule = "Full Day";
            Description = "None";
        }


    }



}
