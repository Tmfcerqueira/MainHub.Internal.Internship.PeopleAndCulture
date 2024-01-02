using MainHub.Internal.PeopleAndCulture.ProjectManagement.Repository.Extensions;
using MainHub.Internal.PeopleAndCulture.ProjectManagement.Data;
using DBModels = MainHub.Internal.PeopleAndCulture.ProjectManagement.Data.Models;
using RepoModels = MainHub.Internal.PeopleAndCulture.ProjectManagement.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace MainHub.Internal.PeopleAndCulture.ProjectManagement.Repository
{
    public sealed class ProjectManagementRepository : IProjectManagementRepository
    {
        private readonly ProjectManagementDbContext _context;

        public ProjectManagementRepository(ProjectManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RepoModels.ProjectActivity>> GetProjectActivities()
        {
            var activities = await _context.ProjectActivities.OrderBy(pa => pa.Name).ToListAsync();
            return activities.ToRepoModel();
        }

        public async Task<IEnumerable<RepoModels.Project>> GetProjectsAsync()
        {
            var projects = await _context.Projects.OrderBy(p => p.Name).ToListAsync();

            return projects.ToRepoModel();
        }
    }
}
