using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models
{
    public class TimesheetResponseModels
    {
        public List<TimesheetResponseModel> Timesheets { get; set; }

        public int AllDataCount { get; set; }

        public TimesheetResponseModels()
        {
            Timesheets = new List<TimesheetResponseModel>();
            AllDataCount = 0;
        }
    }
}
