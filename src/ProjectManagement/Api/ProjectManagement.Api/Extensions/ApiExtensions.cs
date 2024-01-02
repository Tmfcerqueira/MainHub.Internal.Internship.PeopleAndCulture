namespace MainHub.Internal.PeopleAndCulture.ProjectManagement.API.Extensions
{
    using ApiModels = MainHub.Internal.PeopleAndCulture.ProjectManagement.API.Models;
    using RepoModels = MainHub.Internal.PeopleAndCulture.ProjectManagement.Repository.Models;
    public static class ApiExtensions
    {
        public static IEnumerable<ApiModels.Project> ToProjectsApiModel(this IEnumerable<RepoModels.Project> repoProjects)
        {
            List<ApiModels.Project> apiProjects = new List<ApiModels.Project>();

            foreach (RepoModels.Project project in repoProjects)
            {
                ApiModels.Project apiProject = project.ToProjectApiModel();

                apiProjects.Add(apiProject);
            }
            return apiProjects;

        }
        public static ApiModels.Project ToProjectApiModel(this RepoModels.Project repoProject)
        {
            ApiModels.Project project = new ApiModels.Project();
            project.Id = repoProject.Id;
            project.ProjectId = repoProject.ProjectId;
            project.Name = repoProject.Name;
            return project;
        }

        public static IEnumerable<ApiModels.ProjectActivity> ToProjectActivitiesApiModel(this IEnumerable<RepoModels.ProjectActivity> repoProjectActivities)
        {
            List<ApiModels.ProjectActivity> apiProjectActivities = new List<ApiModels.ProjectActivity>();

            foreach (RepoModels.ProjectActivity projectActivity in repoProjectActivities)
            {
                ApiModels.ProjectActivity apiProjectActivity = projectActivity.ToProjectActivityApiModel();

                apiProjectActivities.Add(apiProjectActivity);
            }
            return apiProjectActivities;
        }

        public static ApiModels.ProjectActivity ToProjectActivityApiModel(this RepoModels.ProjectActivity repoProjectActivity)
        {
            ApiModels.ProjectActivity projectActivity = new ApiModels.ProjectActivity();
            projectActivity.Id = repoProjectActivity.Id;
            projectActivity.ProjectActivityId = repoProjectActivity.ProjectActivityId;
            projectActivity.Name = repoProjectActivity.Name;
            return projectActivity;
        }

    }
}
