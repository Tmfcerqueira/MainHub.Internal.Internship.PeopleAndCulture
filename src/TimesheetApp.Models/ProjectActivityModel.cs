using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture
{
    public class ProjectActivityModel
    {
        public Guid ProjectActivityGUID { get; set; }
        public string Name { get; set; }

        public ProjectActivityModel()
        {
            ProjectActivityGUID = Guid.Empty;
            Name = "None";
        }
    }
}
