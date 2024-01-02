using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;

namespace MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models
{
    public class TimesheetActivityCreateRequestModel
    {
        public Guid ActivityGUID { get; set; }
        public Guid TimesheetGUID { get; set; }
        public Guid ProjectGUID { get; set; }
        public TypeOfWork TypeOfWork { get; set; }
        public DateTime ActivityDate { get; set; }
        public int Hours { get; set; }
    }
}
