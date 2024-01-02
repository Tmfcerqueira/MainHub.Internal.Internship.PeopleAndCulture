using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture
{
    public class ActivityProjectModel
    {
        public Guid ActivityGuid { get; set; }
        public string Name { get; set; }

        public ActivityProjectModel()
        {
            ActivityGuid = Guid.Empty;
            Name = "None";
        }
    }
}
