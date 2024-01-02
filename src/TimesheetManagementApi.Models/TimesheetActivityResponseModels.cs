using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models
{
    public class TimesheetActivityResponseModels
    {
        public List<TimesheetActivityResponseModel> Activities { get; set; }

        public int AllDataCount { get; set; }

        public TimesheetActivityResponseModels()
        {
            Activities = new List<TimesheetActivityResponseModel>();
            AllDataCount = 0;
        }
    }
}
