using System;
using System.Reflection;
using MainHub.Internal.PeopleAndCulture.Extensions;
using ProjectManagement.Api.Proxy.Client.Api;
using TimesheetManagement.Api.Proxy.Client.Api;
using TimesheetManagement.Api.Proxy.Client.Client;
using TimesheetManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture
{
    public class TimesheetAppRepository : ITimesheetAppRepository
    {
        //Inject proxies
        public TimesheetAppRepository(ITimesheetApi timesheetProxy, ITimesheetActivityApi timesheetActivityProxy, IAdminApi adminProxy, IProjectApi projectProxy, IProjectActivityApi projectActivityProxy)
        {
            TimesheetProxy = timesheetProxy;
            TimesheetActivityProxy = timesheetActivityProxy;
            AdminProxy = adminProxy;
            ProjectProxy = projectProxy;
            ProjectActivityProxy = projectActivityProxy;
        }
        public ITimesheetApi TimesheetProxy { get; set; }

        public ITimesheetActivityApi TimesheetActivityProxy { get; set; }

        public IAdminApi AdminProxy { get; set; }

        public IProjectApi ProjectProxy { get; set; }

        public IProjectActivityApi ProjectActivityProxy { get; set; }


        //Creates
        public async Task<TimesheetModel> CreateTimesheet(TimesheetModel timesheetModel, Guid actionBy)
        {
            var timesheetRequest = timesheetModel.ToTimesheetCreateRequestModel();

            var response = await TimesheetProxy.ApiPeoplePersonGuidTimesheetPostAsync(timesheetModel.PersonGUID, actionBy, timesheetRequest);

            var finalTimesheetModel = response.ToTimesheetResponseModel();

            return finalTimesheetModel;
        }


        //Create/Update/Delete activities
        public async Task<List<TimesheetActivityModel>> CreateActivities(Guid personGuid, Guid timesheetActivityGuid, Guid actionBy, TimesheetActivityRecordsModel tarm)
        {
            List<TimesheetActivityModel> tams = tarm.ToTimesheetActivitiesModel();

            foreach (var model in tams)
            {
                if (model.TimesheetActivityGUID == Guid.Empty && model.Hours > 0)
                {
                    var timesheetActivityRequest = model.ToTimesheetActivityCreateRequestModel();

                    var response = await TimesheetActivityProxy.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityPostAsync(personGuid, model.TimesheetGUID, personGuid, timesheetActivityRequest);

                    response.ToTimesheetActivityResponseModel();
                }
                else if (model.TimesheetActivityGUID != Guid.Empty && model.Hours > 0)
                {
                    var updatedModel = model.ToTimesheetActivityUpdateModel();

                    await TimesheetActivityProxy.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityTimesheetActivityGuidPutAsync(personGuid, model.TimesheetGUID, model.TimesheetActivityGUID, actionBy, updatedModel);
                }
                else if (model.TimesheetActivityGUID != Guid.Empty && model.Hours == 0)
                {
                    await TimesheetActivityProxy.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityTimesheetActivityGuidDeleteAsync(personGuid, model.TimesheetGUID, model.TimesheetActivityGUID, actionBy);
                }
            }

            return tams;
        }


        //Gets
        public async Task<(List<TimesheetModel>, int errorCode, int count)> GetAllTimesheetsFromOnePerson(Guid personGuid, int year, int month, ApprovalStatus approvalStatus, int page, int pageSize)
        {
            try
            {
                var responseModels = await TimesheetProxy.ApiPeoplePersonGuidTimesheetGetAsync(personGuid, year, month, approvalStatus, page, pageSize);

                List<TimesheetModel> models = responseModels.Timesheets.Select(responseModel => responseModel.ToTimesheetModel()).ToList();

                return (models, 200, responseModels.AllDataCount);
            }
            catch (ApiException ex)
            {
                return (new List<TimesheetModel>(), ex.ErrorCode, 0);
            }
        }

        public async Task<TimesheetModel> GetOneTimesheetFromOnePerson(Guid personGuid, Guid timesheetGuid)
        {
            TimesheetResponseModel responseModel = await TimesheetProxy.ApiPeoplePersonGuidTimesheetTimesheetGuidGetAsync(personGuid, timesheetGuid);

            TimesheetModel model = responseModel.ToTimesheetModel();

            return (model);
        }

        public async Task<(List<TimesheetActivityModel>, int errorCode, int count)> GetAllTimesheetActivitiesForOneTimesheet(Guid personGuid, Guid timesheetGuid)
        {
            try
            {
                var responseModels = await TimesheetActivityProxy.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityGetAsync(personGuid, timesheetGuid);

                List<TimesheetActivityModel> models = responseModels.Activities.Select(responseModel => responseModel.ToTimesheetActivityModel()).ToList();

                return (models, 200, responseModels.AllDataCount);
            }
            catch (ApiException ex)
            {
                return (new List<TimesheetActivityModel>(), ex.ErrorCode, 0);
            }
        }

        public async Task<TimesheetActivityModel> GetOneTimesheetActivityForOneTimesheet(Guid personGuid, Guid timesheetGuid, Guid timesheetActivityGuid)
        {
            TimesheetActivityResponseModel responseModel = await TimesheetActivityProxy.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityTimesheetActivityGuidGetAsync(personGuid, timesheetGuid, timesheetActivityGuid);

            TimesheetActivityModel model = responseModel.ToTimesheetActivityModel();

            return (model);
        }


        public async Task<(List<TimesheetActivityRecordsModel>, int errorCode, int count)> GetTimesheetActivityRecords(Guid personGuid, Guid timesheetGuid)
        {
            try
            {
                var activities = await TimesheetActivityProxy.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityGetAsync(personGuid, timesheetGuid);

                List<TimesheetActivityModel> activityModels = activities.Activities.Select(responseModel => responseModel.ToTimesheetActivityModel()).ToList();

                List<TimesheetActivityRecordsModel> activityRecords = TimesheetActivityRecordsModelExtensions.TimesheetActivityRecords(activityModels);

                return (activityRecords, 200, activities.AllDataCount);
            }
            catch (ApiException ex)
            {
                return (new List<TimesheetActivityRecordsModel>(), ex.ErrorCode, 0);
            }
        }

        public async Task<List<ProjectModel>> GetProjectsForUser(Guid personGuid)
        {
            var projects = await ProjectProxy.ProjectGetAsync();

            List<ProjectModel> projectModels = projects.Select(responseModel => responseModel.ToProjectModel()).ToList();

            return projectModels;
        }

        public async Task<List<ProjectActivityModel>> GetProjectActivities(Guid personGuid, Guid projectGuid)
        {
            var projectActivities = await ProjectActivityProxy.ProjectActivityGetAsync();

            List<ProjectActivityModel> projectActivityModels = projectActivities.Select(responseModel => responseModel.ToProjectActivityModel()).ToList();

            return projectActivityModels;
        }


        //Delete
        public async Task<bool> DeleteTimesheetActivityAsync(Guid personGuid, Guid timesheetGuid, Guid timesheetActivityGuid, Guid actionBy)
        {
            try
            {
                await TimesheetActivityProxy.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityTimesheetActivityGuidDeleteAsync(personGuid, timesheetGuid, timesheetActivityGuid, actionBy);
                return true;
            }
            catch (ApiException)
            {
                return false;
            }
        }

        //Update
        public async Task<bool> UpdateTimesheetAsync(Guid personGuid, Guid timesheetGuid, TimesheetModel updatedModel, Guid actionBy)
        {
            try
            {
                //Call the API to update the timesheet
                TimesheetUpdateModel updateModel = updatedModel.ToTimesheetUpdateModel();
                await TimesheetProxy.ApiPeoplePersonGuidTimesheetTimesheetGuidPutAsync(personGuid, timesheetGuid, actionBy, updateModel);
                return true;
            }
            catch (ApiException)
            {
                return false;
            }
        }


        //History
        public async Task<(List<TimesheetHistoryModel>, int errorCode, int count)> GetTimesheetHistory(Guid timesheetGuid, string personGuid, int page, int pageSize)
        {
            try
            {
                //Call the API to get all timesheets
                var responseModels = await TimesheetProxy.ApiPeoplePersonGuidTimesheetTimesheetGuidHistoryGetAsync(timesheetGuid, personGuid, page, pageSize);

                //Map the list of repo models to list of timesheet models
                var models = responseModels.TimesheetHistory.Select(responseModel => responseModel.ToTimesheetHistoryModel()).ToList();

                return (models, 200, responseModels.AllDataCount);
            }
            catch (ApiException ex)
            {
                return (new List<TimesheetHistoryModel>(), ex.ErrorCode, 0);
            }
        }



        //Admin
        public async Task<(List<TimesheetModel>, int errorCode, int count)> GetAllTimesheetsAsync(int page, int pageSize, ApprovalStatus status)
        {
            try
            {
                //Call the API to get all timesheets
                var responseModels = await AdminProxy.ApiTimesheetGetAsync(page, pageSize, status);

                //Map the list of repo models to list of timesheet models
                List<TimesheetModel> models = responseModels.Timesheets.Select(responseModel => responseModel.ToTimesheetModel()).ToList();

                return (models, 200, responseModels.AllDataCount);
            }
            catch (ApiException ex)
            {
                return (new List<TimesheetModel>(), ex.ErrorCode, 0);
            }
        }
    }
}
