using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;

namespace TimesheetManagement.DataBase.Models
{
    public class TimesheetActivity
    {
        public int TimesheetActivityId { get; set; }
        public Guid TimesheetActivityGUID { get; set; }
        public Guid ActivityGUID { get; set; }
        public Guid TimesheetGUID { get; set; }
        public Guid ProjectGUID { get; set; }
        public TypeOfWork TypeOfWork { get; set; }
        public DateTime ActivityDate { get; set; }
        public int Hours { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid EditedBy { get; set; }
        public Guid DeletedBy { get; set; }

        public virtual Timesheet? Timesheet { get; set; }

        public TimesheetActivity()
        {
            TimesheetActivityId = 0;
            TimesheetActivityGUID = Guid.Empty;
            ActivityGUID = Guid.Empty;
            TimesheetGUID = Guid.Empty;
            ProjectGUID = Guid.Empty;
            TypeOfWork = TypeOfWork.Regular;
            ActivityDate = new DateTime(2999, 12, 31, 23, 59, 59);
            Hours = 0;
            IsDeleted = false;
            CreatedBy = Guid.Empty;
            EditedBy = Guid.Empty;
            DeletedBy = Guid.Empty;
        }
    }
}
