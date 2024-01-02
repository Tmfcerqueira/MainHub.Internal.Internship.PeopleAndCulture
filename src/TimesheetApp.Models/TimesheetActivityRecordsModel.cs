using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;

namespace MainHub.Internal.PeopleAndCulture
{
    public class TimesheetActivityRecordsModel
    {
        public Guid TimesheetActivityGUID { get; set; }
        public Guid TimesheetGUID { get; set; }
        public Guid ProjectGUID { get; set; }
        public Guid ActivityGUID { get; set; }
        public TypeOfWork TypeOfWork { get; set; }
        public ActivityDayModel[] Days { get; set; }
        public TimesheetActivityRecordsModel() : this(0, DateTime.UtcNow.Year, DateTime.UtcNow.Month, Guid.Empty)
        {
        }

        public TimesheetActivityRecordsModel(int hours, int year, int month, Guid timesheetActivityGuid)
        {
            Days = new ActivityDayModel[DateTime.DaysInMonth(year, month)];
            InitActivityDays(hours, year, month, timesheetActivityGuid);
        }

        private void InitActivityDays(int hours, int year, int month, Guid timesheetActivityGuid)
        {
            for (int i = 0; i < DateTime.DaysInMonth(year, month); i++)
            {
                Days[i] = new ActivityDayModel(hours, year, month, i + 1, timesheetActivityGuid);
            }
        }
    }
}
