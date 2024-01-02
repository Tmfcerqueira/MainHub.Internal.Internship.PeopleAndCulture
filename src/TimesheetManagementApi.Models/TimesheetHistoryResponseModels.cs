using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models
{
    public class TimesheetHistoryResponseModels
    {
        public List<TimesheetHistoryResponseModel> TimesheetHistory { get; set; } = new List<TimesheetHistoryResponseModel>();
        public int AllDataCount { get; set; }
    }
}
