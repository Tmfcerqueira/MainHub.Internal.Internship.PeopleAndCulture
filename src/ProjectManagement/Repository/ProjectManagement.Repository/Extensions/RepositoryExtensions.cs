using RepoModels = MainHub.Internal.PeopleAndCulture.ProjectManagement.Repository.Models;
using DBModels = MainHub.Internal.PeopleAndCulture.ProjectManagement.Data.Models;

namespace MainHub.Internal.PeopleAndCulture.ProjectManagement.Repository.Extensions
{
    internal static class RepositoryExtensions
    {
        public static IEnumerable<RepoModels.Project> ToRepoModel(this IEnumerable<DBModels.Project> dbProjects)
        {
            List<RepoModels.Project> repoProjects = new List<RepoModels.Project>();

            foreach (DBModels.Project project in dbProjects)
            {
                RepoModels.Project repoProject = project.ToProjectRepoModel();

                repoProjects.Add(repoProject);
            }
            return repoProjects;
        }

        public static RepoModels.Project ToProjectRepoModel(this DBModels.Project dbProject)
        {
            RepoModels.Project project = new RepoModels.Project();
            project.Id = dbProject.Id;
            project.ProjectId = dbProject.ProjectId;
            project.Name = dbProject.Name;
            return project;
        }

        public static IEnumerable<RepoModels.ProjectActivity> ToRepoModel(this IEnumerable<DBModels.ProjectActivity> dbProjectActivities)
        {
            List<RepoModels.ProjectActivity> repoProjectActivities = new List<RepoModels.ProjectActivity>();

            foreach (DBModels.ProjectActivity projectActivity in dbProjectActivities)
            {
                RepoModels.ProjectActivity repoProjectActivity = projectActivity.ToProjectActivityRepoModel();

                repoProjectActivities.Add(repoProjectActivity);
            }
            return repoProjectActivities;
        }

        public static RepoModels.ProjectActivity ToProjectActivityRepoModel(this DBModels.ProjectActivity dbProject)
        {
            RepoModels.ProjectActivity projectActivity = new RepoModels.ProjectActivity();
            projectActivity.Id = dbProject.Id;
            projectActivity.ProjectActivityId = dbProject.ProjectActivityId;
            projectActivity.Name = dbProject.Name;
            return projectActivity;
        }


    }
}
