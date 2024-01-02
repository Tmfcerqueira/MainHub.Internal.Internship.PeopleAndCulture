using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class ProjectActivityRepoModel
    {
        public static ProjectActivityModel ToProjectActivityModel(this ProjectActivity model)
        {

            return new ProjectActivityModel
            {
                ProjectActivityGUID = model.ProjectActivityId,
                Name = model.Name,
            };
        }
    }
}
