using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;

namespace MainHub.Internal.PeopleAndCulture.Models
{
    public class TimesheetActivityHistory
    {
        public int TimesheetActivityId { get; set; }
        public Guid TimesheetActivityGUID { get; set; }
        public Guid ActivityGUID { get; set; }
        public Guid TimesheetGUID { get; set; }
        public Guid ProjectGUID { get; set; }
        public TypeOfWork TypeOfWork { get; set; }
        public DateTime ActivityDate { get; set; }
        public int Hours { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        public Guid UserGUID { get; set; }

        public TimesheetActivityHistory()
        {
            TimesheetActivityId = 0;
            TimesheetActivityGUID = Guid.Empty;
            ActivityGUID = Guid.Empty;
            TimesheetGUID = Guid.Empty;
            ProjectGUID = Guid.Empty;
            TypeOfWork = TypeOfWork.Regular;
            ActivityDate = new DateTime(2999, 12, 31, 23, 59, 59);
            Hours = 0;
            Action = "None";
            ActionDate = new DateTime(2999, 12, 31, 23, 59, 59);
            UserGUID = Guid.Empty;
        }
    }
}
