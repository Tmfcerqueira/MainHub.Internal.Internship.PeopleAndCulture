using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoModelsModels = MainHub.Internal.PeopleAndCulture.ProjectManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture
{
    public interface IProjectManagementRepository
    {
        Task<IEnumerable<RepoModelsModels.Project>> GetProjectsAsync();
        Task<IEnumerable<RepoModelsModels.ProjectActivity>> GetProjectActivities();
    }
}
