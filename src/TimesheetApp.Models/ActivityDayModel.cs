using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture
{
    public class ActivityDayModel
    {
        public Guid TimesheetActivityGUID { get; set; }
        public DateTime ActivityDate { get; set; }
        public int Hours { get; set; }

        public ActivityDayModel(int hours, DateTime activityDate, Guid timesheetActivityGuid)
        {
            TimesheetActivityGUID = timesheetActivityGuid;
            Hours = hours;
            ActivityDate = activityDate;
        }
        public ActivityDayModel(int hours, int year, int month, int day, Guid timesheetActivityGuid) : this(hours, new DateTime(year, month, day), timesheetActivityGuid)
        {
        }
    }
}
