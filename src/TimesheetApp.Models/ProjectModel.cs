using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;

namespace MainHub.Internal.PeopleAndCulture
{
    public class ProjectModel
    {
        public Guid ProjectGUID { get; set; }
        public string Name { get; set; }

        public ProjectModel()
        {
            ProjectGUID = Guid.Empty;
            Name = "None";
        }
    }
}
