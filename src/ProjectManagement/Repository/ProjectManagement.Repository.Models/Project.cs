using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.ProjectManagement.Repository.Models
{
    public sealed class Project
    {
        public int Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public Project()
        {
            Id = 0;
            ProjectId = Guid.Empty;
            Name = "None";
        }
    }
}
