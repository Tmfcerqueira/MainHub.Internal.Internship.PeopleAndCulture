using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimesheetManagement.DataBase.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public Guid ProjectGUID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TimesheetActivity>? TimesheetActivities { get; set; }

        public Project()
        {
            ProjectId = 0;
            ProjectGUID = Guid.Empty;
            Name = "None";
        }
    }
}
