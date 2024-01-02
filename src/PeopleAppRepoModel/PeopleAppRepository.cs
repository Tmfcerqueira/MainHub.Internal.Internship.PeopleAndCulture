using System.Drawing.Printing;
using System.Net;
using App.Models;
using MainHub.Internal.PeopleAndCulture;
using MainHub.Internal.PeopleAndCulture.App.Repository.Extensions;
using MainHub.Internal.PeopleAndCulture.Extensions;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using PeopleManagement.Api.Proxy.Client.Api;
using PeopleManagement.Api.Proxy.Client.Model;
using PeopleManagementDataBase;

namespace App.Repository
{
    public class PeopleAppRepository : IPeopleAppRepository
    {
        public GraphServiceClient GraphServiceClient { get; set; }
        public ICollaboratorApi CollaboratorProxy { get; set; }
        public PeopleAppRepository(ICollaboratorApi collaboratorProxy, GraphServiceClient graphServiceClient)
        {
            this.CollaboratorProxy = collaboratorProxy;
            GraphServiceClient = graphServiceClient;
        }
        public async Task<PeopleModel> CreateCollaboratorAsync(PeopleModel peopleModel)
        {
            if (peopleModel.FirstName == "" || peopleModel.LastName == "" || peopleModel.Email == "")
            {
                return null!;
            }
            else
            {
                //normal to request
                var collaboratorRequest = peopleModel.ToPeopleCreateRequestModel();
                //call proxy
                var response = await CollaboratorProxy.CreateCollaboratorAsyncAsync(collaboratorRequest);
                //response to PeopleModel
                var finalPeopleModel = response.ToPeopleModel();

                return finalPeopleModel;
            }
        }

        public async Task<(List<AllPeopleModel>, int count)> GetAllCollaborators(int page, int pageSize, string filter, State list)
        {
            var collaborator = await CollaboratorProxy.GetAllCollaboratorsAsync(page, pageSize, filter, (PeopleManagement.Api.Proxy.Client.Model.State)list);
            var collaboratorMapped = collaborator.Collaborators.Select(c => c.ToAllPeopleModel()).ToList();
            return (collaboratorMapped, collaborator.TotalCount);
        }

        public async Task<PeopleModel> GetOneCollaborator(Guid peoplemodelId)
        {
            try
            {
                var collaboratorRepo = await CollaboratorProxy.ApiPeopleIdGetAsync(peoplemodelId);
                var collaboratorModel = collaboratorRepo.ToOnePeopleModel();
                return collaboratorModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null!;
            }
        }

        public async Task<bool> DeleteCollaboratorAsync(Guid personId)
        {

            await CollaboratorProxy.ApiPeoplePersonIdDeleteAsync(personId);
            return true;
        }

        public async Task<bool> UpdateCollaboratorAsync(Guid personId, PeopleModel collaborator)
        {
            try
            {
                ApiCollaboratorUpdateModel updatedCollaborator = collaborator.ToUpdateModel();
                updatedCollaborator.Contact ??= string.Empty;
                updatedCollaborator.EmergencyContact ??= string.Empty;
                updatedCollaborator.Iban ??= string.Empty;
                await CollaboratorProxy.ApiPeoplePersonIdPutAsync(personId, updatedCollaborator);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<PeopleHistory>> GetCollaboratorHistory(Guid id, int page, int pageSize)
        {
            var collaboratorHistory = await CollaboratorProxy.ApiPeopleIdHistoryGetAsync(id, page, pageSize);
            var collaboratorMapped = collaboratorHistory.Select(c => c.ToPeopleHistory()).ToList();
            return collaboratorMapped;
        }


        public async Task<List<AllPeopleModel>> GetBirthdays()
        {
            int page = 1;
            int pageSize = 20;
            var collaborators = await CollaboratorProxy.GetAllCollaboratorsAsync(page, pageSize, string.Empty, PeopleManagement.Api.Proxy.Client.Model.State.All);
            var currentDate = DateTime.Now;
            var endDate = currentDate.AddDays(7);

            var collaboratorsBirthday = collaborators
                .Collaborators
                .Where(
                    c => (c.BirthDate.Month == endDate.Month && c.BirthDate.Day <= endDate.Day)
                        || (c.BirthDate.Month < endDate.Month)
                ).ToList();


            var collaboratorMapped = collaboratorsBirthday.Select(c => c.ToAllPeopleModel());

            return collaboratorMapped.ToList();
        }



        public async Task<(List<AllPeopleModel>, int count)> GetAzure(int page, int pageSize, string filter)
        {
            int skip = (page - 1) * pageSize;
            int take = pageSize;
            int resultCount = 0;
            int count = 0;
            var collaborator = await CollaboratorProxy.GetAllCollaboratorsAsync(1, 500, string.Empty, PeopleManagement.Api.Proxy.Client.Model.State.All);
            var result = await GraphServiceClient.Users.Request().GetAsync();
            List<AllPeopleModel> finalResult = new List<AllPeopleModel>();
            List<AllPeopleModel> filteredResult = new List<AllPeopleModel>();
            foreach (var item in result)
            {
                if (!collaborator.Collaborators.Exists(c => c.Email == item.Mail))
                {
                    finalResult.Add(item.ToAllPeopleModel());
                    resultCount += 1;
                }
            }
            if (!filter.IsNullOrEmpty())
            {
                foreach (var item in finalResult)
                {
                    if (item.FirstName!.ToLower().Contains(filter.ToLower()) || item.LastName!.ToLower().Contains(filter.ToLower()))
                    {
                        filteredResult.Add(item);
                        count += 1;
                    }
                }
                var collaboratorPaged = filteredResult
                .OrderBy(c => c.FirstName)
                .ThenBy(c => c.LastName)
                .Skip(skip)
                .Take(take)
                .ToList();
                return (collaboratorPaged, count);
            }
            else
            {
                count = resultCount;
                var collaboratorPaged = finalResult
                .OrderBy(c => c.FirstName)
                .ThenBy(c => c.LastName)
                .Skip(skip)
                .Take(take)
                .ToList();
                return (collaboratorPaged, count);
            }
        }

        public async Task<PeopleModel> ImportAzure(Guid id, AllPeopleModel allPeopleModel)
        {
            try
            {
                var collaborator = allPeopleModel.ToPeopleModel();
                var response = await CollaboratorProxy.CreateCollaboratorAsyncAsync(collaborator.ToPeopleCreateRequestModel());
                return response.ToPeopleModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null!;
            }
        }

    }
}
