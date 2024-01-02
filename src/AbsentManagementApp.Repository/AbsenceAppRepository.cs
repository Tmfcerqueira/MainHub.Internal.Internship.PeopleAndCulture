using AbsentManagement.Api.Proxy.Client.Api;
using AbsentManagement.Api.Proxy.Client.Client;
using AbsentManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.App.Repository.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text;
using Microsoft.Identity.Web;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using MainHub.Internal.PeopleAndCulture.App.Models;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Printing;
using System;
using MainHub.Internal.PeopleAndCulture.Extensions;
using App.Models;
using App.Repository;
using PeopleManagement.Api.Proxy.Client.Api;
using PeopleProxyModel = PeopleManagement.Api.Proxy.Client.Model;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models
{
    public class AbsenceAppRepository : IAbsenceAppRepository
    {

        private List<AbsenceTypeModel> Types { get; set; }

        //inject proxies
        public AbsenceAppRepository(IAbsenceApi absenceProxy, IAbsenceTypeApi absenceTypeProxy, IAdminApi adminProxy, ICollaboratorApi collaboratorProxy)
        {
            AbsenceProxy = absenceProxy;
            AbsenceTypeProxy = absenceTypeProxy;
            AdminProxy = adminProxy;
            CollaboratorProxy = collaboratorProxy;

            List<AbsenceTypeResponseModel> responseModels = AbsenceTypeProxy.ApiAbsenceTypeTypesGetAsync().Result;
            List<AbsenceTypeModel> models = responseModels.Select(responseModel => responseModel.ToAbsenceTypeModel()).ToList();

            Types = models;
        }

        public IAbsenceApi AbsenceProxy { get; set; }

        public IAbsenceTypeApi AbsenceTypeProxy { get; set; }

        public IAdminApi AdminProxy { get; set; }

        public ICollaboratorApi CollaboratorProxy { get; set; }


        //create

        public async Task<(AbsenceModel, int)> CreateAbsenceAsync(AbsenceModel absenceModel, Guid actionBy)
        {
            try
            {
                // Check if the absence period overlaps with any existing absences
                bool isOverlapping = await CheckAbsenceOverlapAsync(absenceModel.PersonGuid, absenceModel.AbsenceStart, absenceModel.AbsenceEnd);
                if (isOverlapping)
                {
                    return (new AbsenceModel(), 400); // Return a 400 Bad Request status code indicating the overlap
                }

                // convert from normal to request
                var absenceRequest = absenceModel.ToAbsenceCreateRequestModel();

                // use the proxy to call the API
                var response = await AbsenceProxy.ApiPeoplePersonGuidAbsencePostAsync(absenceModel.PersonGuid, actionBy, absenceRequest);

                // convert from response to absenceModel
                var finalAbsenceModel = response.ToAbsenceModel();

                // return normal model
                return (finalAbsenceModel, 200);
            }
            catch (ApiException ex)
            {
                return (new AbsenceModel(), ex.ErrorCode);
            }
        }

        public async Task<bool> CheckAbsenceOverlapAsync(Guid personGuid, DateTime startDate, DateTime endDate)
        {
            var existingAbsences = (new List<AbsenceModel>(), 200, 0);

            (List<AbsenceModel>, int, int) result;
            int resultCount;
            var count = 0;
            List<AbsenceModel> resultAppList = new List<AbsenceModel>();
            var pageSize = 20;
            var page = 1;

            result = await GetAbsencesByPersonAsync(personGuid, DateTime.Now.Year, page, pageSize, ApprovalStatus.All, new DateTime(), new DateTime());
            resultCount = result.Item3;
            resultAppList.AddRange(result.Item1);
            count += result.Item1.Count;
            while (count < resultCount)
            {
                page++;
                result = await GetAbsencesByPersonAsync(personGuid, DateTime.Now.Year, page, pageSize, ApprovalStatus.All, new DateTime(), new DateTime());
                resultAppList.AddRange(result.Item1);
                count += result.Item1.Count;
            }

            existingAbsences.Item1 = resultAppList;

            // Check if the new absence period overlaps with any existing absence
            foreach (var existingAbsence in existingAbsences.Item1)
            {
                if (startDate <= existingAbsence.AbsenceEnd && endDate >= existingAbsence.AbsenceStart && existingAbsence.ApprovalStatus != Common.ApprovalStatus.Rejected)
                {
                    return true; // Overlap detected  
                }
            }

            return false; // No overlap found
        }

        //gets
        public async Task<(List<AbsenceModel>, int errorCode, int count)> GetAbsencesByPersonAsync(Guid person, int year, int page, int pageSize, ApprovalStatus status, DateTime startDate, DateTime endDate)
        {
            try
            {
                // Call the API to get all absences
                var responseModels = await AbsenceProxy.ApiPeoplePersonGuidAbsenceGetAsync(person, year, page, pageSize, status, startDate, endDate);

                // Map the list of repo models to list of absence models
                List<AbsenceModel> models = responseModels.Absences.Select(responseModel => responseModel.ToAbsenceModel()).ToList();

                // Set the type string from other table
                foreach (var model in models)
                {
                    SetTypeTextByGuidAsync(model);
                }

                return (models, 200, responseModels.AllDataCount);
            }
            catch (ApiException ex)
            {
                return (new List<AbsenceModel>(), ex.ErrorCode, 0);
            }
        }

        public async Task<(List<AbsenceHistoryModel>, int errorCode, int count)> GetAbsenceHistory(Guid id, string personGuid, int page, int pageSize)
        {
            try
            {
                // Call the API to get all absences
                var responseModels = await AbsenceProxy.ApiPeoplePersonGuidAbsenceIdHistoryGetAsync(id, personGuid, page, pageSize);

                // Map the list of repo models to list of absence models
                var models = responseModels.AbsenceHistory.Select(responseModel => responseModel.ToAbsenceHistoryModel()).ToList();

                // Set the type string from other table
                foreach (var model in models)
                {
                    SetHistoryTypeTextByGuidAsync(model);
                }

                return (models, 200, responseModels.AllDataCount);
            }
            catch (ApiException ex)
            {
                return (new List<AbsenceHistoryModel>(), ex.ErrorCode, 0);
            }
        }

        public async Task<(AbsenceModel, int)> GetAbsenceByPersonAsync(Guid personGuid, Guid absenceId)
        {
            try
            {
                // Call the API to get the absence by ID
                AbsenceResponseModel responseModel = await AbsenceProxy.ApiPeoplePersonGuidAbsenceAbsenceIdGetAsync(personGuid, absenceId);

                // Map the repo model to an absence model
                AbsenceModel model = responseModel.ToAbsenceModel();

                // Set the type string from the absence type table
                SetTypeTextByGuidAsync(model);

                return (model, 200);
            }
            catch (ApiException ex)
            {
                return (new AbsenceModel(), ex.ErrorCode);
            }
        }


        public async Task<List<AbsenceTypeModel>> GetAllAbsenceTypesAsync()
        {
            //Get the absence types from the AbsenceTypeProxy
            List<AbsenceTypeResponseModel> responseModels = await AbsenceTypeProxy.ApiAbsenceTypeTypesGetAsync();

            // Map the list of repo models to list of absence type models
            List<AbsenceTypeModel> models = responseModels.Select(responseModel => responseModel.ToAbsenceTypeModel()).ToList();

            return models;
        }


        //delete

        public async Task<bool> DeleteAbsenceAsync(Guid personGuid, Guid absenceGuid, Guid actionBy)
        {
            try
            {
                // Call the API to delete the absence by ID and person
                await AbsenceProxy.ApiPeoplePersonGuidAbsenceAbsenceIdDeleteAsync(personGuid, absenceGuid, actionBy);
                return true;
            }
            catch (ApiException)
            {
                return false;
            }
        }


        //update
        public async Task<bool> UpdateAbsenceAsync(Guid personGuid, Guid absenceId, AbsenceModel updatedModel, Guid actionBy)
        {
            try
            {
                // Call the API to update the absence
                AbsenceUpdateModel updateModel = updatedModel.ToAbsenceUpdateModel();
                await AbsenceProxy.ApiPeoplePersonGuidAbsenceAbsenceIdPutAsync(personGuid, absenceId, actionBy, updateModel);
                return true;
            }
            catch (ApiException)
            {
                return false;
            }
        }



        //aux methods


        public async Task<bool> SubmitAllDraftAsync(Guid personGuid, Guid actionBy)
        {
            try
            {

                var existingAbsences = (new List<AbsenceResponseModel>(), 200, 0);

                AbsenceResponseModels result;
                int resultCount;
                var count = 0;
                List<AbsenceResponseModel> resultAppList = new List<AbsenceResponseModel>();
                var pageSize = 20;
                var page = 1;

                result = await AbsenceProxy.ApiPeoplePersonGuidAbsenceGetAsync(personGuid, DateTime.Now.Year, page, pageSize, ApprovalStatus.Draft, new DateTime(), new DateTime());
                resultCount = result.AllDataCount;
                resultAppList.AddRange(result.Absences);
                count += result.Absences.Count;
                while (count < resultCount)
                {
                    page++;
                    result = await AbsenceProxy.ApiPeoplePersonGuidAbsenceGetAsync(personGuid, DateTime.Now.Year, page, pageSize, ApprovalStatus.Draft, new DateTime(), new DateTime());
                    resultAppList.AddRange(result.Absences);
                    count += result.Absences.Count;
                }

                existingAbsences.Item1 = resultAppList;

                foreach (var absence in existingAbsences.Item1)
                {
                    var updatedAbsence = absence.ToAbsenceUpdateModel();
                    updatedAbsence.ApprovalStatus = ApprovalStatus.Submitted;
                    updatedAbsence.SubmissionDate = DateTime.Now;

                    await AbsenceProxy.ApiPeoplePersonGuidAbsenceAbsenceIdPutAsync(personGuid, absence.AbsenceGuid, actionBy, updatedAbsence);
                }

                return true;
            }
            catch (ApiException)
            {
                return false;
            }
        }


        public void SetTypeTextByGuidAsync(AbsenceModel model)
        {

            List<AbsenceTypeModel> types = Types;

            //set the type string from other table
            foreach (var type in types)
            {
                if (type.TypeGuid == model.AbsenceTypeGuid)
                {
                    model.AbsenceType = type.Type;
                    break;
                }
            }


        }


        public void SetTypeTextByGuidAsync(AbsenceAdminModel model)
        {
            List<AbsenceTypeModel> types = Types;

            //set the type string from other table
            foreach (var type in types)
            {
                if (type.TypeGuid == model.AbsenceTypeGuid)
                {
                    model.AbsenceType = type.Type;
                    break;
                }
            }
        }

        public void SetHistoryTypeTextByGuidAsync(AbsenceHistoryModel model)
        {
            List<AbsenceTypeModel> types = Types;

            //set the type string from other table
            foreach (var type in types)
            {
                if (type.TypeGuid == model.AbsenceTypeGuid)
                {
                    model.AbsenceType = type.Type;
                    break;
                }
            }
        }

        //admin
        public async Task<(List<AbsenceModel>, int errorCode, int count)> GetAllAbsencesAsync(int page, int pageSize, ApprovalStatus status)
        {
            try
            {
                // Call the API to get all absences
                var responseModels = await AdminProxy.ApiAbsenceGetAsync(page, pageSize, status);

                // Map the list of repo models to list of absence models
                List<AbsenceModel> models = responseModels.Absences.Select(responseModel => responseModel.ToAbsenceModel()).ToList();

                // Set the type string from other table
                foreach (var model in models)
                {
                    SetTypeTextByGuidAsync(model);
                }

                return (models, 200, responseModels.AllDataCount);
            }
            catch (ApiException ex)
            {
                return (new List<AbsenceModel>(), ex.ErrorCode, 0);
            }
        }


        public async Task<(List<AllPeopleModel>, int count)> GetAllPeople(int page, int pageSize, string filter, State list)
        {
            var collaborator = await CollaboratorProxy.GetAllCollaboratorsAsync(page, pageSize, filter, (PeopleProxyModel.State)list);
            var collaboratorMapped = collaborator.Collaborators.Select(c => c.ToAllPeopleModel()).ToList();
            return (collaboratorMapped, collaborator.TotalCount);
        }


    }
}
