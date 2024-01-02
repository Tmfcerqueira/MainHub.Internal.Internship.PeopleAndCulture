using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;

namespace TimesheetManagement.DataBase.Models
{
    public class Timesheet
    {
        public int TimesheetId { get; set; }
        public Guid TimesheetGUID { get; set; }
        public Guid PersonGUID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public DateTime DateOfApproval { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid EditedBy { get; set; }
        public Guid DeletedBy { get; set; }

        public virtual ICollection<TimesheetActivity>? TimesheetActivities { get; set; }

        public Timesheet()
        {
            TimesheetId = 0;
            TimesheetGUID = Guid.Empty;
            PersonGUID = Guid.Empty;
            Month = 0;
            Year = 0;
            ApprovalStatus = ApprovalStatus.Draft;
            ApprovedBy = "None";
            DateOfSubmission = new DateTime(2999, 12, 31, 23, 59, 59);
            DateOfApproval = new DateTime(2999, 12, 31, 23, 59, 59);
            IsDeleted = false;
            CreatedBy = Guid.Empty;
            EditedBy = Guid.Empty;
            DeletedBy = Guid.Empty;
        }
    }
}
