using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models
{
    public class TimesheetRepoModel
    {
        public Guid TimesheetGUID { get; set; }
        public Guid PersonGUID { get; set; }
        public string PersonName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public DateTime DateOfApproval { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid EditedBy { get; set; }
        public Guid DeletedBy { get; set; }
        public bool IsDeleted { get; set; }

        public TimesheetRepoModel()
        {
            TimesheetGUID = Guid.Empty;
            PersonGUID = Guid.Empty;
            PersonName = "None";
            Month = 0;
            Year = 0;
            ApprovalStatus = ApprovalStatus.Draft;
            ApprovedBy = "None";
            DateOfSubmission = new DateTime(2999, 12, 31, 23, 59, 59);
            DateOfApproval = new DateTime(2999, 12, 31, 23, 59, 59);
            CreatedBy = Guid.Empty;
            EditedBy = Guid.Empty;
            DeletedBy = Guid.Empty;
            IsDeleted = false;
        }
    }
}
