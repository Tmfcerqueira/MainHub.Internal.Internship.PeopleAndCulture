using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.ProjectManagement.Data.Models
{
    public sealed class ProjectActivity
    {
        public int Id { get; set; }
        public Guid ProjectActivityId { get; set; }
        public string Name { get; set; }
        public ProjectActivity()
        {
            Id = 0;
            ProjectActivityId = Guid.Empty;
            Name = "None";
        }
    }
}
