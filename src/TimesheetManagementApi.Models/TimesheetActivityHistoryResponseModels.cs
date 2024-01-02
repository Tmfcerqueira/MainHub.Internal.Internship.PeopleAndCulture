using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models
{
    public class TimesheetActivityHistoryResponseModels
    {
        public List<TimesheetActivityHistoryResponseModel> TimesheetActivityHistory { get; set; } = new List<TimesheetActivityHistoryResponseModel>();
        public int AllDataCount { get; set; }
    }
}
