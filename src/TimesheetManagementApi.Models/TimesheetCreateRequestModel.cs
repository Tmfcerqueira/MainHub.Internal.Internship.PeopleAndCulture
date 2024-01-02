using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.Properties
{
    public class TimesheetCreateRequestModel
    {
        public Guid PersonGUID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
