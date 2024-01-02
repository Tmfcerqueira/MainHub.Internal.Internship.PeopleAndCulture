using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class ProjectRepoModel
    {
        public static ProjectModel ToProjectModel(this Project model)
        {

            return new ProjectModel
            {
                ProjectGUID = model.ProjectId,
                Name = model.Name,
            };
        }
    }
}
